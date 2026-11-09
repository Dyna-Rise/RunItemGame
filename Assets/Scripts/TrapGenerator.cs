using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    //レーン番号はなし
    const float StageChipSize = 30.0f; //ステージの奥行

    public float minInterval = 5.0f; //トラップの間隔の最小値
    public float maxInterval = 15.0f;//トラップの間隔の最大値
    public float offsetZ = 5.0f; //ずらして置く位置
    public float posY = 0.5f; //トラップを設置する高さ
    public float spawnRate = 0.3f; //トラップの出る割合

    public GameObject trapPrefab; //生成したいトラップ

    void Start()
    {
        for (float z = -StageChipSize / 2 + offsetZ; z < StageChipSize / 2; z += Random.Range(minInterval, maxInterval))
        {
            //外れたら次のループへ
            if (Random.value > spawnRate) continue;

            //生成位置
            Vector3 pos = transform.position + new Vector3(0, posY, z);

            //ステージの子どもとして生成
            Instantiate(trapPrefab, pos, Quaternion.identity, transform);
        }
    }
}
