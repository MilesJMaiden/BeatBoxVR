using System.Collections;
using UnityEngine;

public class GamePauseController : MonoBehaviour
{
    public GameObject countdownUI;

    public void PauseGame()
    {
        // Pause game logic
        Time.timeScale = 0;
        // Show pause menu... Call from UI controller
    }

    public void UnpauseGameWithCountdown()
    {
        StartCoroutine(CountdownAndUnpause());
    }

    private IEnumerator CountdownAndUnpause()
    {
        // Show countdown UI
        countdownUI.SetActive(true);
        yield return new WaitForSecondsRealtime(3); // 3-second countdown

        countdownUI.SetActive(false);
        Time.timeScale = 1; // Resume game
    }
}
