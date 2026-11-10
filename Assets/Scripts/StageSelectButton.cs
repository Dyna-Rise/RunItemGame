using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StageSelectButton : MonoBehaviour
{
    //行き先とシーン名
    public string nextScene;

    void Start()
    {
        //ボタンの子オブジェクトの文字をいったん消す
        GetComponentInChildren<TextMeshProUGUI>().text = "";

        //クリアしていたら★をつける
        if (GameData.clearStages.Contains(nextScene))
        {
            GetComponentInChildren<TextMeshProUGUI>().text = "★";
        }
        //行き先となるシーン名をボタンのテキストとする
        GetComponentInChildren<TextMeshProUGUI>().text += nextScene;
    }
    //行き先にジャンプする
    public void Departure()
    {
        SceneManager.LoadScene(nextScene);
    }
}
