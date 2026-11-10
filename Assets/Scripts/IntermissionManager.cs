using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntermissionManager : MonoBehaviour
{
    [Header("所持リスト")]
    public GameObject upgradeButtonPrefab; //所持アイテム1つ分のボタン（プレハブ）
    public Transform listParent; //ボタンを並べる親（Vertical Layout Groupが付いたパネル）

    [Header("スロット・説明")]
    public TextMeshProUGUI[] slotTexts; //スロットの表示（GameData.SlotCount個ぶん）
    public TextMeshProUGUI descriptionText; //選んだアイテムの説明

    [Header("テスト用（このシーンから再生したとき用の所持アイテム）")]
    public UpgradeData[] testUpgrades;

    [Header("キー操作")]
    public GameObject firstStageButton; //アイテムを1つも持っていない時に選んでおくステージボタン

    UpgradeData lastSelected; //最後に押したアイテム（作り直した後もそこを選び直すため）


    void Start()
    {
        //Intermissionシーンから始めた時はアイテムを保持していない
        //テスト用のアイテムを所持リストに加える（空なら何もしない）
        foreach (UpgradeData data in testUpgrades)
        {
            GameData.AddUpgrade(data);
        }

        descriptionText.text = "アイテムを選んでスロットにセットしよう";
        Refresh(); //画面を最新の状態にする
    }

    //画面を最新の状態にする（ボタンを全部消して作り直す）
    void Refresh()
    {
        //① 所持リストのボタンをいったん全部消す
        foreach (Transform child in listParent)
        {
            Destroy(child.gameObject);
        }

        GameObject selectTarget = null; //キー操作で選んでおくボタン

        //② 所持しているアイテムの数だけボタンを作る
        foreach (UpgradeData data in GameData.ownedUpgrades)
        {
            GameObject obj = Instantiate(upgradeButtonPrefab, listParent);

            //選んでおくボタンを決める（基本は先頭、さっき押したアイテムがあればそれ）
            if (selectTarget == null) selectTarget = obj;
            if (data == lastSelected) selectTarget = obj;

            //ボタンの文字（セット中なら印を付ける）
            string label = data.upgradeName;
            if (GameData.equippedUpgrades.Contains(data)) label = "★ " + label;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = label;

            
            //ボタンが押されたら、OnClickUpgradeButtonを呼ぶ
            //引数指定ができないのでちょっとまわりくどい
            //OnClickUpgradeButtonメソッド→SelectUpgradeメソッド

            //obj.GetComponent<Button>().onClick.AddListener(OnClickUpgradeButton);


            //ボタンが押されたとき、ラムダ式「() =>」でメソッドを指名する手法にすることで、
            //現在リストから取り出したdataを引数にSelectUpgradeを呼び出せる
            obj.GetComponent<Button>().onClick.AddListener(() => SelectUpgrade(data));
        }

        //アイテムを1つも持っていなければ、ステージボタンを選んでおく
        if (selectTarget == null) selectTarget = firstStageButton;
        //キー操作（Navigate/Submit）で動かせるように、ボタンを「選択中」にする
        //※ボタンを作り直すと選択が消えるので、毎回選び直す
        EventSystem.current.SetSelectedGameObject(selectTarget);

        //③ スロットの表示
        for (int i = 0; i < slotTexts.Length; i++)
        {
            if (i < GameData.equippedUpgrades.Count)
            {
                slotTexts[i].text = GameData.equippedUpgrades[i].upgradeName;
            }
            else
            {
                slotTexts[i].text = "（空き）";
            }
        }
    }

    // ラムダ式でない時、ボタンが押されたときに呼ばれるメソッド
    // 何番目のアイテムがおされたのかを判別し、その番号と対応する
    // ownedUpgradesリストのデータを引数にさらにSelectUpgradeを発動
    //void OnClickUpgradeButton()
    //{
    //    // 1. 今押されたボタンのGameObjectを取得する
    //    GameObject clickedObj = EventSystem.current.currentSelectedGameObject;
    //    if (clickedObj == null) return;

    //    // 2. そのボタンが親（listParent）の中で「上から何番目か」を取得する（ownedUpgradesリストと一致しているはず）
    //    int index = clickedObj.transform.GetSiblingIndex();

    //    // 3. 番号に対応するアイテムを取り出して処理する
    //    UpgradeData data = GameData.ownedUpgrades[index];
    //    SelectUpgrade(data);
    //}

    //所持リストのボタンが押されたとき
    void SelectUpgrade(UpgradeData data)
    {
        lastSelected = data; //押したアイテムを覚えておく

        //説明文を表示
        descriptionText.text = data.upgradeName + "\n" + data.description;

        if (GameData.equippedUpgrades.Contains(data))
        {
            //セット中なら外す
            GameData.equippedUpgrades.Remove(data);
        }
        else if (GameData.equippedUpgrades.Count < GameData.SlotCount)
        {
            //スロットに空きがあればセット
            GameData.equippedUpgrades.Add(data);
        }
        else
        {
            //スロットがいっぱい
            descriptionText.text = "スロットがいっぱいです。\nセット中のアイテムを押すと外せます";
        }

        Refresh(); //画面を更新
    }
}
