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

    void Start()
    {
        gameState = GameState.IsPlay;
    }

    void Update()
    {
        
    }
}
