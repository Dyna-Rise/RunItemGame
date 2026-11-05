using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //ゲームのステータス用のテキスト
    public GameObject statusText;

    //スライダー関連
    public GameObject sliderPanel;
    public Slider lifeSlider;

    //バレット関連
    public GameObject bulletPanel;
    public TextMeshProUGUI bulletText;

    //ボタン関連
    public GameObject buttonPanel;
    public GameObject retryButton;

    void Start()
    {
        //ボタンを非表示
        buttonPanel.SetActive(false);

        //時間差でステータス文字を非表示
        StartCoroutine(DisplayChange());

        UpdateLife(); //プレイヤー体力を反映
        UpadateBullet(); //残弾数を反映
    }

    //時間差でステータス文字を非表示
    IEnumerator DisplayChange()
    {
        yield return new WaitForSeconds(1.0f);
        statusText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ライフUIを更新
    public void UpdateLife()
    {
        //PlayerRunスクリプトを参照できるようにする
        PlayerRun player = GameObject.FindAnyObjectByType<PlayerRun>();

        //残Lifeをスライダー値に代入
        lifeSlider.value = player.Life();
    }

    //Bulletの残弾数を更新
    public void UpadateBullet()
    {
        //PlayerShooterスクリプトを参照できるようにする
        PlayerShooter player = GameObject.FindAnyObjectByType<PlayerShooter>();

        //残弾を文字列に変換してテキストに代入
        bulletText.text = player.GetRemaining().ToString();
    }
}
