using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("弾の自動消滅時間")]
    public float deleteTime = 3.0f;

    void Start()
    {
        //生成と同時にdeleteTime秒後に消滅
        Destroy(gameObject, deleteTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameData.HasUpgrade(UpgradeType.PierceBullet) && other.gameObject.tag == "Enemy")
        {
            return;
        }

        //何かにぶつかったら消滅
        Destroy(gameObject);
    }
}
