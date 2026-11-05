using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

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
    public PlayableDirector timeline; 

    void Awake()
    {
        //mainカメラのCinemachineBrainを無効化
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
    }

    void Start()
    {
        gameState = GameState.IsPlay;
    }

    void Update()
    {
        if (gameState == GameState.GameOver)
        {
            Debug.Log("Gameover");
            //UIControllerを探し出してゲームオーバー用のUI表示命令
            UIController contoller = GameObject.FindAnyObjectByType<UIController>();
            contoller.GameOverDisplay();

            Camera.main.GetComponent<CinemachineBrain>().enabled = true;

            //GetComponent<CameraTransition>().OnGameOver();

            timeline.Play();

            gameState = GameState.GameEnd;
            Debug.Log("GameEnd");
        }
    }
}
