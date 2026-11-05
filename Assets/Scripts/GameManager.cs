using System.Collections;
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
    GameObject virtualCamera;

    void Start()
    {
        gameState = GameState.IsPlay;
        virtualCamera = GameObject.FindGameObjectWithTag("ViartualCamera");
        virtualCamera.SetActive(false);
    }

    void Update()
    {
        if(gameState == GameState.GameOver)
        {
            UIController controller = GameObject.FindAnyObjectByType<UIController>();
            controller.GameOverDisplay();


            virtualCamera.SetActive(true);
          Camera.main.GetComponent<CinemachineBrain>().enabled = true;

            gameState = GameState.GameEnd;
        }
    }

    IEnumerator DemoCamera()
    {
        yield return new WaitForSeconds(1.0f);

    }
}
