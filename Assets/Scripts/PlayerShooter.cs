using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("生成される弾")]
    public GameObject bullet;

    InputAction shootAction; //Attackアクションボタンの割り振り

    Coroutine shootStart; //シュートされているかどうかを知るための変数

    [Header("弾速・インターバル時間")]
    public float bulletSpeed = 10.0f;
    public float shootInterval = 0.5f;

    const int startRemaining = 10; //初期弾数
    private int bulletRemaining = startRemaining; //弾の残数
                                                  
    //UIなどに残数を伝えるアクセサメソッド
    public int GetRemaining()
    {
        return bulletRemaining;
    }

    //弾数を回復するメソッド
    public void AddBullet(int num)
    {
        bulletRemaining += num;
    }

    void Start()
    {
        //PlayerInputコンポーネントに設定されている現Mapを取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        //現マップ上にいる「Attack」というアクションを取得
        shootAction = actionMap.FindAction("Attack");
    }

    void Update()
    {
        //Attackアクションボタンが押された時 かつ 残弾があれば　Shootメソッド
        if (shootAction.WasPerformedThisFrame() && bulletRemaining > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shootStart == null)
        {
            bulletRemaining--; //残弾を１減らす

            UIController controller = GameObject.FindAnyObjectByType<UIController>();
            controller.UpdateBulletRemaining();

            //攻撃開始Bulletプレハブの生成とＺ軸前方へシュート
            GameObject obj = Instantiate(   //生成と同時にそれを変数に代入しておく
                bullet,                     //生成対象オブジェクト
                transform.position,         //生成場所※細かく指定したければGateオブジェなど用意
                Quaternion.identity);       //生成した時のRotation

            Rigidbody rbody = obj.GetComponent<Rigidbody>(); //生成したもののRigidbody
            rbody.AddForce(new Vector3(0, 0, bulletSpeed), ForceMode.Impulse); //前方に押し出す

            //インターバルとなるコルーチンの開始
            shootStart = StartCoroutine(ShootStart());
        }
    }

    //インターバル用コルーチン
    IEnumerator ShootStart()
    {
        //0.5ｆ秒待つ
        yield return new WaitForSeconds(shootInterval);

        //コルーチン変数の解放
        shootStart = null;
    }
}
