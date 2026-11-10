using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public string nextScene; //NewGameボタンで移動するシーン名

    public string loadScene = "Intermisson"; //インターミッション
    public Button loadButton; //Loadボタンがおせないこともあるようにする

    public UpgradeData[] allUpgrades; //ゲーム中に存在する強化アイテムデータ達

    //スタートボタンのOnClickから呼び出されるメソッド
    public void NewGame()
    {
        GameData.ResetData(); //セーブデータをリセット

        SceneManager.LoadScene(nextScene);
    }

    //ロードボタン用のメソッド
    public void LoadGame()
    {
        //データを呼び出す
        GameData.Load(allUpgrades);

        //Intermission画面にジャンプ
        SceneManager.LoadScene(loadScene);
    }
}
