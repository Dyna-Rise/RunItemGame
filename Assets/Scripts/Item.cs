using UnityEngine;

public enum ItemType
{
    Life,
    Bullet,
    Upgrade
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int value;
    public UpgradeData upgradeData; //生成したデータの情報を参照できる

    private void OnTriggerEnter(Collider other)
    {
        //相手がプレイヤーなら
        if(other.gameObject.tag == "Player")
        {
            //タイプによってやることが異なる
            switch (itemType)
            {
                case ItemType.Life:
                    other.gameObject.GetComponent<PlayerRun>().RecoverLife(value);
                    FindAnyObjectByType<UIController>().UpdateLife(); //UI更新
                    break;
                case ItemType.Bullet:
                    other.gameObject.GetComponent<PlayerShooter>().AddBullet(value);
                    FindAnyObjectByType<UIController>().UpdateBullet();　//Bullet更新
                    break;
                case ItemType.Upgrade:
                    //管理リストに加える
                    break;
            }
            Destroy(gameObject);
        }
    }
}
