using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
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

    //private GameObject mixingCamera;

    public CinemachineCamera[] cameras;

    public PlayableDirector playableDirector;

    void Start()
    {
        gameState = GameState.IsPlay;

        //mixingカメラを探索
        //mixingCamera = GameObject.FindGameObjectWithTag("MixngCamera");
        //mixingカメラを非表示
        //mixingCamera.SetActive(false);
        //mainカメラのCinemachineBrainを無効化
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
    }

    void Update()
    {
        if (gameState == GameState.GameOver)
        {
            //UIControllerを探し出してゲームオーバー用のUI表示命令
            UIController contoller = GameObject.FindAnyObjectByType<UIController>();
            contoller.GameOverDisplay();

            //mixingCamera.SetActive(true); //mixingカメラを復活
            Camera.main.GetComponent<CinemachineBrain>().enabled = true; //Brainを復活

            //StartCoroutine(GameOverCamera());

            playableDirector.Play(); //意図的に作っておいたTimelineを再生する

            gameState = GameState.GameEnd;
        }
    }

    IEnumerator GameOverCamera()
    {
        yield return null; //1フレーム待ってから

        cameras[1].Priority = 2;
    }
}
