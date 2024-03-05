using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject drumKit; // Reference to the drum kit
    public GameObject countdownUI; // Reference to the countdown UI
    public GameObject pauseUI; // Reference to the pause UI
    public CanvasGroup mainMenuCanvasGroup; // Reference to the main menu's canvas group

    public float animationDuration = 1.0f; // Duration for animations and fades

    private bool isGamePaused = false;

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            // Pause logic
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            FadeCanvasGroup(mainMenuCanvasGroup, 1, animationDuration);
        }
        else
        {
            // Unpause logic
            StartCoroutine(CountdownAndUnpause());
        }
    }

    private IEnumerator CountdownAndUnpause()
    {
        // Show countdown UI
        pauseUI.SetActive(false);
        countdownUI.SetActive(true);
        yield return new WaitForSecondsRealtime(3); // 3-second countdown

        countdownUI.SetActive(false);
        FadeCanvasGroup(mainMenuCanvasGroup, 0, animationDuration);
        Time.timeScale = 1; // Resume game
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Additional logic after fade if needed
    }

    // Additional methods for UI interactions can be added here
}
