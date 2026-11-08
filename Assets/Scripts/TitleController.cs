using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public string nextScene; //NewGameボタンで移動するシーン名

    //スタートボタンのOnClickから呼び出されるメソッド
    public void NewGame()
    {
        SceneManager.LoadScene(nextScene);
    }
}
