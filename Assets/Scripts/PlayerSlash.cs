using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlash : MonoBehaviour
{    
    public GameObject slashObject; //対象スラッシュオブジェクト

    InputAction slashAction; //スラッシュのためのアクションを参照

    [Header("スラッシュ攻撃のインターバル")]
    public float slashTime = 0.5f;

    Coroutine slashStart; //スラッシュ発生コルーチンを参照する変数

    void Start()
    {   
        //PlayerInputコンポーネントに設定されている現Mapを取得
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        //現マップ上にいる「Crouch」というアクションを取得
        slashAction = actionMap.FindAction("Crouch");

        slashObject.SetActive(false); //最初は必ずスラッシュオブジェクトを非表示
    }

    void Update()
    {
        if (GameManager.gameState == GameState.GameEnd) return;

        //該当ボタンが押されたとき
        if (slashAction.WasPerformedThisFrame())
        {
            Slash(); //スラッシュ攻撃メソッドの発動
        }
    }

    void Slash()
    {
        if(slashStart == null) //すでに攻撃中でなければ
        {
            slashObject.SetActive(true); //スラッシュとなるオブジェクトを表示

            slashStart = StartCoroutine(SlashStart()); //攻撃コルーチンの発動
        }
    }

    IEnumerator SlashStart()
    {
        yield return new WaitForSeconds(slashTime); //変数で決めた秒数待つ
        slashObject.SetActive(false); //スラッシュオブジェクトを非表示
        slashStart = null; //次の攻撃ができるようにコルーチン解放
    }
}
