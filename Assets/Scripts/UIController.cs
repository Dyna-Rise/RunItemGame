using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public GameObject statusText;
    
    public GameObject sliderPanel;
    public Slider lifeSlider;

    public GameObject bulletPanel;
    public TextMeshProUGUI bulletText;
    
    public GameObject buttonPanel;
    public GameObject retryButton;

    void Start()
    {
        buttonPanel.SetActive(false);
        UpdateLife();
        UpdateBulletRemaining();

        StartCoroutine(StartStatus());
    }

    public void GameOverDisplay()
    {
        statusText.SetActive(true);
        statusText.GetComponent<TextMeshProUGUI>().text = "GAME OVER";
        buttonPanel.SetActive(true);

        sliderPanel.SetActive(false);
        bulletPanel.SetActive(false);
    }

    public void UpdateLife()
    {
        PlayerRun playerRun = GameObject.FindAnyObjectByType<PlayerRun>();

        lifeSlider.value = playerRun.Life();
    }

    public void UpdateBulletRemaining()
    {
        PlayerShooter playerShooter = GameObject.FindAnyObjectByType<PlayerShooter>();

        bulletText.text = playerShooter.GetRemaining().ToString();
    }

    IEnumerator StartStatus()
    {
        yield return new WaitForSeconds(1.0f);
        statusText.SetActive(false);
    }
}
