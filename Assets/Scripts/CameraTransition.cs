using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraTransition : MonoBehaviour
{
    public CinemachineCamera[] cameras;

    /// <summary>
    /// ゲームオーバー演出の開始
    /// </summary>
    public void OnGameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        // Brainに「現在のLiveカメラはカメラ1だ」と認識させる
        yield return null;

        // 3. カメラ2の優先度を上げて、カメラ1からカメラ2へのブレンド（3秒）を開始させる
        cameras[1].Priority = 20;

        yield return new WaitForSeconds(4.0f);

        cameras[2].Priority = 30;

        yield return new WaitForSeconds(1.0f);

        cameras[0].Priority = 40;

    }
}