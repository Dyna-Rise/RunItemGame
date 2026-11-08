using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //相手がプレイヤーなら
        if (other.gameObject.tag == "Player")
        {
            //ステータスをゲームクリアに変更（あとはGameManagerにおまかせ）
            GameManager.gameState = GameState.GameClear;
        }
    }
}
