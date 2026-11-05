using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int enemyLife = 3; //敵の体力
    public GameObject enemyBody; //見た目のオブジェクトの情報

    //無敵時間の秒数
    public float freezeTime = 1.0f;

    //ダメージ中のコルーチンを参照する変数
    Coroutine damageStart;

    void Update()
    {
        //もしもダメージコルーチン変数がnullでなければ(ダメージ中）
        if (damageStart != null)
        {
            //点滅処理
            //valが一定周期で正か負か 値を決める
            float val = Mathf.Sin(Time.time * 50);//三角関数と時間
            if(val > 0)
            {
                //見た目のMeshRendererをON
                enemyBody.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                //見た目のMeshRendererをOFF
                enemyBody.GetComponent<MeshRenderer>().enabled = false;
            }

            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //ダメージコルーチンが発動していなければ（ダメージ中でなければ）
        if(damageStart == null)
        {
            //ぶつかった相手がBulletなら
            if (other.gameObject.tag == "Bullet")
            {
                enemyLife--; //体力が1減る
                if (enemyLife <= 0)
                {
                    Destroy(gameObject); //消滅
                }
                else //体力が残っていれば
                {
                    damageStart = StartCoroutine(DamageStart()); //コルーチンの発動
                }
            }
            //ぶつかった相手がSlashなら即撃破
            else if(other.gameObject.tag == "Slash")
            {
                Destroy(gameObject);
            }
        }
    }

    //時間差でダメージ状態を解くコルーチン
    IEnumerator DamageStart()
    {
        //freezeTimeまってから、ダメージコルーチン変数を解放
        yield return new WaitForSeconds(freezeTime);
        damageStart = null;

        //見た目のMeshRendererをON
        enemyBody.GetComponent<MeshRenderer>().enabled = true;
    }
}
