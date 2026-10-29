using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 diff; //維持すべき対象とカメラの距離の差

    //public GameObject target; //対象となるオブジェクト
    public float followSpeed = 5.0f; //追従スピード

    GameObject player; //対象となるPlayer

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //Playerを検索
        //diff = target.transform.position - transform.position;    
        diff = player.transform.position - transform.position;  //最初に維持すべき距離を定義する
    }

    //Playerが動いた後にカメラの位置を決めたい
    void LateUpdate()
    {
        //カメラがPlayerの動きに遅れて追いつくようにしたい（Lerpメソッドで距離を補完していくように動かす）

        //現状A→目的地Bに向けて無限に収束していく、
        //現状Aの位置はどんどん変化するのでそれに対する進捗分だけどんどん進んでいく
        //最終的には限りなく0に近づくだけだが、描画上は完全に止まっているように表現
        transform.position = Vector3.Lerp(
            transform.position, //現状A
            player.transform.position - diff, //目的地B
            Time.deltaTime * followSpeed //A→Bへの進行に対して、どれだけ進むかの進捗を指定
        );

    }
}
