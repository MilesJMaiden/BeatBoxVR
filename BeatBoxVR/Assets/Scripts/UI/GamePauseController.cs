using System.Collections;
using UnityEngine;

public class GamePauseController : MonoBehaviour
{
    public GameObject countdownUI; // Reference to countdown UI element

    public void PauseGame()
    {
        // Pause game logic
        Time.timeScale = 0;
        // Show pause menu...
    }

    public void UnpauseGameWithCountdown()
    {
        StartCoroutine(CountdownAndUnpause());
    }

    private IEnumerator CountdownAndUnpause()
    {
        // Show countdown UI and wait
        countdownUI.SetActive(true);
        // Countdown logic...
        yield return new WaitForSecondsRealtime(3); // 3-second countdown

        countdownUI.SetActive(false);
        Time.timeScale = 1; // Resume game
    }
}
