using Unity.Cinemachine;
using UnityEngine;

//自作の列挙型（ゲームのステータス）
public enum GameState
{
    None,
    IsPlay,
    GameClear,
    GameOver,
    GameEnd
}

public class GameManager : MonoBehaviour
{
    //自作した型を扱うStatic変数
    public static GameState gameState;

    private GameObject mixingCamera;

    void Start()
    {
        gameState = GameState.IsPlay;

        //mixingカメラを探索
        mixingCamera = GameObject.FindGameObjectWithTag("MixngCamera");
        //mixingカメラを非表示
        mixingCamera.SetActive(false);
        //mainカメラのCinemachineBrainを無効化
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
    }

    void Update()
    {
        if (gameState == GameState.GameOver)
        {
            Debug.Log("Gameover");
            //UIControllerを探し出してゲームオーバー用のUI表示命令
            UIController contoller = GameObject.FindAnyObjectByType<UIController>();
            contoller.GameOverDisplay();

            mixingCamera.SetActive(true); //mixingカメラを復活
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;

            gameState = GameState.GameEnd;
            Debug.Log("GameEnd");
        }
    }
}
