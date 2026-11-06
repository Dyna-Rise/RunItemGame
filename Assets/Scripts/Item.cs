using UnityEngine;

public enum ItemType
{
    Life,
    Bullet
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int value;

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
                    break;
                case ItemType.Bullet:
                    other.gameObject.GetComponent<PlayerShooter>().AddBullet(value);
                    break;
            }

        }
    }
}
