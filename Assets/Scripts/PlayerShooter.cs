using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("生成される弾")]
    public GameObject bullet;

    InputAction shootAction; //Attackアクションボタンの割り振り

    Coroutine shootStart; //シュートされているかどうかを知るための変数

    void Start()
    {
        //PlayerInputコンポーネントに設定されている現Mapを取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        //現マップ上にいる「Attack」というアクションを取得
        shootAction = actionMap.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        //Attackアクションボタンが押された時 Shootメソッド
    }

    void Shoot()
    {
        if (shootStart == null)
        {
            //攻撃開始Bulletプレハブの生成とＺ軸前方へシュート

            //インターバルとなるコルーチンの開始
        }
    }

    //インターバル用コルーチン
    IEnumerator ShootStart()
    {
        //0.5ｆ秒待つ

        //コルーチン変数の解放
    }
}
