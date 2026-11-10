using System.Collections.Generic;
using UnityEngine;

//シーンをまたいで情報を持ち続けるためのクラス
//static（MonoBehaviourではない）なので、オブジェクトに付ける必要はない
public static class GameData
{
    public const int SlotCount = 2; //セットできるスロットの数

    //入手済みの拡張アイテム
    public static List<UpgradeData> ownedUpgrades = new List<UpgradeData>();

    //スロットにセット中の拡張アイテム（最大SlotCount個）
    public static List<UpgradeData> equippedUpgrades = new List<UpgradeData>();

    //クリアしたシーン名を管理
    public static List<string> clearStages = new List<string>();

    //拡張アイテムを入手する（同じものは2つ持たない）
    public static void AddUpgrade(UpgradeData data)
    {
        if (!ownedUpgrades.Contains(data))
        {
            ownedUpgrades.Add(data);
        }
    }

    //その種類の拡張アイテムをセットしているかどうか
    public static bool HasUpgrade(UpgradeType type)
    {
        foreach (UpgradeData data in equippedUpgrades)
        {
            if (data.upgradeType == type) return true;
        }
        return false;
    }

    //その種類の拡張アイテムの効果の大きさ（セットしていなければ0）
    public static int GetValue(UpgradeType type)
    {
        int total = 0;
        foreach (UpgradeData data in equippedUpgrades)
        {
            if (data.upgradeType == type) total += data.value;
        }
        return total;
    }

    //===== セーブ・ロード =====

    //PlayerPrefsに保存するときの名前（キー）
    const string ClearStagesKey = "ClearStages";
    const string UpgradesKey = "OwnedUpgrades";

    //データをまっさらにする（NewGameやロードの前に使う）
    public static void ResetData()
    {
        ownedUpgrades.Clear();
        equippedUpgrades.Clear();
        clearStages.Clear();
    }

    //セーブデータがあるかどうか
    public static bool HasSave()
    {
        return PlayerPrefs.HasKey(ClearStagesKey);
    }

    //セーブ（ステージクリアのたびにGameManagerから呼ばれる）
    public static void Save()
    {
        //クリアしたステージ名を「,」でつないで1つの文字列にして保存
        //例："BaseStage,Stage2"
        PlayerPrefs.SetString(ClearStagesKey, string.Join(",", clearStages));

        //所持アイテムはアセットの名前を「,」でつないで保存
        //例："LifeUp,PierceBullet"
        List<string> names = new List<string>();
        foreach (UpgradeData data in ownedUpgrades)
        {
            names.Add(data.name);
        }
        PlayerPrefs.SetString(UpgradesKey, string.Join(",", names));

        PlayerPrefs.Save(); //書き込みを確定
    }

    //ロード（allUpgradesは、ゲームに登場する全ての拡張アイテム）
    public static void Load(UpgradeData[] allUpgrades)
    {
        ResetData(); //いったん空にしてから読み込む

        //「,」で区切って、クリアしたステージ名を1つずつリストに戻す
        string stages = PlayerPrefs.GetString(ClearStagesKey, "");
        foreach (string stage in stages.Split(','))
        {
            if (stage != "") clearStages.Add(stage);
        }

        //「,」で区切った名前と同じ名前の拡張アイテムを探して、所持リストに戻す
        string upgrades = PlayerPrefs.GetString(UpgradesKey, "");
        foreach (string upgradeName in upgrades.Split(','))
        {
            foreach (UpgradeData data in allUpgrades)
            {
                if (data.name == upgradeName) AddUpgrade(data);
            }
        }
    }
}
