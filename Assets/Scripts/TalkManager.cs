using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TalkManager : MonoBehaviour
{
    InputAction submitAction;

    public TalkData currentTalkData; // インスペクターからTalkDataアセットをアタッチ

    // UI側のパーツ
    [Header("UI設定")]
    public GameObject talkPanel;        // 会話枠パネル
    public TextMeshProUGUI nameText;     // 名前UI
    public TextMeshProUGUI messageText;  // 本文UI
    public Image faceImage;             // 顔グラフィックUI

    [Header("演出設定")]
    [Tooltip("1文字が表示される秒数（小さいほど速い）")]
    public float textSpeed = 0.04f;

    private int currentIndex = -1; // 現在表示している会話の番号
    private bool isTyping = false;         // 1文字送り中かどうか
    private string currentFullText = "";   // 現在表示中の全文
    private Coroutine typingCoroutine;     // 文字送りコルーチン参照

    public string nextScene;

    void Start()
    {

        //PlayerInputコンポーネントに設定されている現Mapは何かを取得＝（UIマップ）
        InputActionMap actionMap = GetComponent<PlayerInput>().currentActionMap;
        submitAction = actionMap.FindAction("Submit"); //現マップ上にいる”Submit”というアクションを取得

        AdvanceTalk(); // 入力があったらすべてこのメソッド1つに集約
    }

    void Update()
    {
        if (submitAction.WasPressedThisFrame())
        {
            AdvanceTalk(); // 入力があったらすべてこのメソッド1つに集約
        }
    }

    public void AdvanceTalk()
    {
        // 会話データがない場合は何もしない
        if (currentTalkData == null || currentTalkData.talks.Count == 0) return;

        // 【状態1】文字送り中の場合 -> 即座に全文表示（スキップ処理）
        if (isTyping)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            messageText.text = currentFullText; // 全文を表示
            isTyping = false;
            return;
        }

        // 【状態2】全文表示済みの場合 -> 次のセリフへ進む
        currentIndex++;

        // 会話リストの最後まで到達した場合は終了
        if (currentIndex >= currentTalkData.talks.Count)
        {
            EndTalk();
            return;
        }

        // 会話パネルをアクティブ化（初回開始時など）
        if (talkPanel != null && !talkPanel.activeSelf)
        {
            talkPanel.SetActive(true);
        }

        // データの読み込み
        SingleTalk talk = currentTalkData.talks[currentIndex];

        // 名前と画像の反映
        if (nameText != null) nameText.text = talk.name;
        if (faceImage != null)
        {
            faceImage.gameObject.SetActive(talk.faceImage != null);
            faceImage.sprite = talk.faceImage;
        }

        // 文字送りコルーチンを開始
        currentFullText = talk.message;
        typingCoroutine = StartCoroutine(TypeTextCoroutine(currentFullText));
    }

    IEnumerator TypeTextCoroutine(string text)
    {
        isTyping = true;
        messageText.text = "";

        foreach (char c in text)
        {
            messageText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    private void EndTalk()
    {
        isTyping = false;
        currentIndex = -1; // 再度開始できるようにリセット

        if (talkPanel != null)
        {
            talkPanel.SetActive(false);
        }

        StartCoroutine(NextScene());

    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(nextScene);
    }

}
