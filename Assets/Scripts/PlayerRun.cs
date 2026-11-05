using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRun : MonoBehaviour
{
    const int MinLane = -2; //最小レーン番号
    const int MaxLane = 2; //最大レーン番号
    const float LaneWidth = 2.0f; //レーン幅
    const int DefaultLife = 3; //最大体力
    const float StunDuration = 0.5f; //硬直時間

    CharacterController controller; //CharacterControllerを参照できる変数
    [SerializeField] private GameObject animatorBody; //Animatorコンポーネントをもっているオブジェクトの取得
    Animator animator; //Animatorを参照できる変数

    //Vector3 moveDirection = new Vector3(0, 0, 0);
    Vector3 moveDirection = Vector3.zero; //Moveメソッドに与える方向
    int targetLane; //目指すべきレーン番号

    int life = DefaultLife; //現体力
    float recoverTime = 0.0f; //硬直カウントダウン

    [Header("重力・前進・横移動・ジャンプ")]
    public float gravity; //重力値
    public float speedZ; //前に進む力
    public float speedX; //横移動スピード
    public float speedJump; //ジャンプ力

    [Header("前進加速力")]
    public float accelerationZ = 10.0f; //加速力

    [Header("ノックバック設定")]
    public float knockbackForceZ = -6.0f; // 後ろに弾かれる強さ（マイナス値）

    Coroutine resetIntervalCol; //インプットのインターバル置くための変数

    private InputAction moveAction; //"Move"アクションを参照するための変数
    private InputAction jumpAction; //"Jump"アクションを参照するための変数

    //[Header("プレイヤーの見た目オブジェクト")]
    //public GameObject playerBody;

    //体力の残を参照できるメソッド
    public int Life()
    {
        return life;
    }

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
        if (GameManager.gameState == GameState.GameEnd) return;

        //点滅処理
        if (IsStun())
        {
            float val = Mathf.Sin(Time.time * 50);

            if (val > 0)
            {
                animatorBody.SetActive(true);
            }
            else
            {
                animatorBody.SetActive(false);
            }
        }
        else //ダメージ中じゃない時
        {
            //姿を表す
            animatorBody.SetActive(true);
        }


        //CharacterControllerコンポーネントが接地判断をもっている
        //入力検知は地面にいる時
        if (controller.isGrounded)
        {
            //上下キーによる前か後ろかの値の設定
            //if (Input.GetAxis("Vertical") != 0.0f)
            //if(moveAction.ReadValue<Vector2>().y != 0.0f)
            //{
            //    //moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            //    moveDirection.z = moveAction.ReadValue<Vector2>().y * speedZ;
            //}
            //else
            //{
            //    moveDirection.z = 0;
            //}

            //横回転 Rotateメソッドで直接回転させる
            //transform.Rotate(0, Input.GetAxis("Horizontal") * 300 * Time.deltaTime, 0);
            //transform.Rotate(0, moveAction.ReadValue<Vector2>().x * 300 * Time.deltaTime, 0);

            //コルーチンが発動していない時だけ入力処理
            if (resetIntervalCol == null)
            {
                if (moveAction.ReadValue<Vector2>().x < 0) MoveToLeft(); //左のレーン番号に変更
                else if (moveAction.ReadValue<Vector2>().x > 0) MoveToRight(); //右のレーン番号に変更
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
                //対象とするレーンをもとにmoveDirectionのX(横移動)を決める
                float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
                moveDirection.x = ratioX * speedX;

                //自動前進するためのmoveDirectionのZ(前進加速)を決める
                float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
                moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

            }

            //ジャンプボタン　押されている間
            //if (Input.GetButton("Jump"))
            if (jumpAction.IsPressed())
            {
                if (IsStun()) return; //スタン中は何も起こさない

                moveDirection.y = speedJump; //y方向をセッティング
                //animator.SetTrigger("jump"); //ジャンプアニメへの切替
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

    public void MoveToLeft()
    {
        if (IsStun()) return; //スタン中は何も起こさない

        //現レーンがまだ最小でなければ
        if (targetLane > MinLane)
        {
            targetLane--; //対象レーン番号をひとつ減らす
            //次の入力認識するまでのインターバルを保つコルーチンの発動
            resetIntervalCol = StartCoroutine(ResetIntervalCol());
        }
    }

    public void MoveToRight()
    {
        if (IsStun()) return; //スタン中は何も起こさない

        //現レーンがまだ最大でなければ
        if (targetLane < MaxLane)
        {
            targetLane++; //対象レーン番号をひとつ増やす
        }
    }

    //特定の時間（インターバル）だけ入力を待つコルーチン
    IEnumerator ResetIntervalCol()
    {
        yield return new WaitForSeconds(0.1f); //0.1秒待ってから
        resetIntervalCol = null; //コルーチン発動中である証明だった変数を解放
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

        //UIControllerを探して、ライフ更新メソッド
        UIController controller = GameObject.FindAnyObjectByType<UIController>();
        controller.UpdateLife();

        //ライフが0以下になったら
        if(life <= 0)
        {
            //ステータス変更
            GameManager.gameState = GameState.GameOver;
        }
    }
}
