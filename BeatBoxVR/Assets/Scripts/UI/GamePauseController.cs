using System.Collections;
using UnityEngine;

public class GamePauseController : MonoBehaviour
{
    public GameObject countdownUI;
    public GameObject pauseUI;

    public void PauseGame()
    {
        // Pause game logic
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    public void UnpauseGameWithCountdown()
    {
        StartCoroutine(CountdownAndUnpause());
    }

    private IEnumerator CountdownAndUnpause()
    {
        // Show countdown UI
        pauseUI.SetActive(false);
        countdownUI.SetActive(true);
        yield return new WaitForSecondsRealtime(3); // 3-second countdown

        countdownUI.SetActive(false);
        Time.timeScale = 1; // Resume game


    }
}
