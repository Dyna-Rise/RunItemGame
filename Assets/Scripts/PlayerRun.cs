using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerRun : MonoBehaviour
{
    const int DefaultLife = 3; //最大体力
    const float StunDuration = 0.5f; //硬直時間

    const int MinLane = -2; //最小レーン番号
    const int MaxLane = 2; //最大レーン番号
    const float LaneWidth = 2.0f; //レーン幅

    CharacterController controller; //CharacterControllerを参照できる変数
    Animator animator; //Animatorを参照できる変数

    [Header("対象アニメオブジェクト")]
    [SerializeField] private GameObject animatorBody; //Animatorコンポーネントをもっているオブジェクトの取得

    Vector3 moveDirection = Vector3.zero; //Moveメソッドに与える方向
    int targetLane; //目標レーン番号

    int life = DefaultLife; //現体力
    float recoverTime = 0.0f; //硬直カウントダウン

    [Header("重力・前進・横移動・ジャンプ")]
    public float gravity = 20.0f; //重力加速値
    public float speedZ = 5.0f; //前進スピード
    public float speedX = 3.0f; //横移動スピード
    public float speedJump = 8.0f; //ジャンプ力

    [Header("前進加速力")]
    public float accelerationZ = 10.0f; //加速値

    Coroutine resetIntervalCol; //インプットのインターバルコルーチン

    private InputAction moveAction; //"Move"アクションを参照するための変数
    private InputAction jumpAction; //"Jump"アクションを参照するための変数

    [Header("ノックバック設定")]
    public float knockbackForceZ = -6.0f; // 後ろに弾かれる強さ（マイナス値）

    //硬直カウントダウン中か体力0なら止まる
    bool IsStun()
    {
        return recoverTime > 0.0f || life <= 0;
    }

    void Start()
    {
        //Playerについているコンポーネントの取得
        controller = GetComponent<CharacterController>();
        animator = animatorBody.GetComponent<Animator>();

        //PlayerInputコンポーネントに設定されている現Mapは何かを取得＝（Playerマップ）
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        moveAction = actionMap.FindAction("Move"); //現マップ上にいる”Move”というアクションを取得
        jumpAction = actionMap.FindAction("Jump"); //現マップ上にいる”Jump”というアクションを取得
    }

    void Update()
    {
        //CharacterControllerコンポーネントが接地判断をもっている
        //入力検知は地面にいる時
        if (controller.isGrounded)
        {
            //入力インターバルを担当するコルーチンが発動している間は左右移動を連続作動できない
            if (resetIntervalCol == null)
            {
                if (moveAction.ReadValue<Vector2>().x < 0) MoveToLeft(); //左の入力があれば
                else if (moveAction.ReadValue<Vector2>().x > 0) MoveToRight(); //右の入力があれば
            }

            //ジャンプボタン　押されている間
            if (jumpAction.IsPressed())
            {
                //animator.SetTrigger("jump"); //ジャンプアニメへの切替
                Jump();
            }

            if (IsStun()) //硬直中なら何もできない
            {
                moveDirection.x = 0.0f; //横方向には動けない
                //Z方向は0.05fずつノックバックが解消されるが、最大でも0で止まる
                moveDirection.z = Mathf.Min(moveDirection.z + 0.05f, 0.0f);

                recoverTime -= Time.deltaTime; //カウントダウンする
            }
            else
            {
                //自動前進するためのmoveDirectionのZ(前進加速)を決める
                float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
                moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

                //対象とするレーンをもとにmoveDirectionのX(横移動)を決める
                float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
                moveDirection.x = ratioX * speedX;
            }
        }

        //常に重力はかかっている
        moveDirection.y -= gravity;

        //セッティングしたmoveDirectionの値を使ってMoveメソッドの発動
        //※実際にPlayerが動く Time.deltaTimeでPC性能差によらないように動作安定
        //回転を含む前進だと、グローバルな座標指定からズレるので、グローバルな座標になるように変換してからMoveに与える
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //地面にいるのであればY成分の力を毎回0にリセット
        if (controller.isGrounded) moveDirection.y = 0;

        //アニメの制御
        //animator.SetBool("run", moveDirection.z != 0.0f);

    }

    //対象レーンを左にずらすためのメソッド
    public void MoveToLeft()
    {
        if (IsStun()) return; //スタン中は何も起こさない

        //現レーンがまだ最小でなければ
        if (targetLane > MinLane)
        {
            targetLane--; //対象レーン番号をひとつ減らす
            resetIntervalCol = StartCoroutine(ResetIntervalCol()); //インターバル分だけ入力待ちを設けるコルーチン
        }
    }

    //対象レーンを右にずらすためのメソッド
    public void MoveToRight()
    {
        if (IsStun()) return; //スタン中は何も起こさない

        //現レーンがまだ最大でなければ
        if (targetLane < MaxLane)
        {
            targetLane++; //対象レーン番号をひとつ増やす
            resetIntervalCol = StartCoroutine(ResetIntervalCol()); //インターバル分だけ入力待ちを設けるコルーチン
        }
    }

    //インターバル分だけ入力待ちを設けるコルーチン
    IEnumerator ResetIntervalCol()
    {
        yield return new WaitForSeconds(0.1f); //0.1秒待ち
        resetIntervalCol = null; //0.1秒の足止めの後、コルーチン発動中であることを示す変数を解放
    }

    //ジャンプメソッド
    public void Jump()
    {
        moveDirection.y = speedJump; //上方向に値を代入
    }

    //Playerの接触判定
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (IsStun()) return; //スタン中なら二重にイベントは起こさない

        //ぶつかった相手がEnemyなら
        if (hit.gameObject.tag == "Enemy")
        {
            moveDirection.z = knockbackForceZ; //ノックバック：後ろに弾かれる力が最初にかかる
            LifeDown(); //体力減少
            recoverTime = StunDuration; //硬直時間をrecoverTimeにセット ※ここがダメージ硬直の始まり
        }
    }

    //体力のダメージによる減少
    public void LifeDown()
    {
        life--;
    }

}
