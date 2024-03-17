using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    //public GameObject drumKit; // Reference to the drum kit

    public static bool IsGamePaused = false;

    //UI
    public GameObject countdownUI; // Reference to the countdown UI
    public TextMeshProUGUI countdownText;
    public GameObject pauseUI; // Reference to the pause UI
    public CanvasGroup mainMenuCanvasGroup; // Reference to the main menu's canvas group
    public float animationDuration = 1.0f; // Duration for animations and fades

    //Game Modes
    public GameObject playAlongModePrefab;
    public GameObject tutorialModePrefab;
    public GameObject playModePrefab;

    // Deactivate all game modes on start to ensure a clean slate
    private void Start()
    {
        DeactivateAllModes();
    }

    public void TogglePauseGame()
    {
        IsGamePaused = !IsGamePaused;
        PlayModeManager.Instance.UnpauseTracksAndNotes(IsGamePaused); // Pass the pause state
    }

    private void PauseGame()
    {
        // Logic to pause the game
        Time.timeScale = 0;
        pauseUI.SetActive(true);
        // Ensure music and note movement are paused (handled in PlayModeManager)
        PlayModeManager.Instance.PauseTracksAndNotes();
    }

    private IEnumerator CountdownAndUnpause()
    {
        // Hide pause UI and show countdown
        pauseUI.SetActive(false);
        countdownUI.SetActive(true);

        // Perform the countdown while game is still paused in terms of game logic
        for (int count = 3; count > 0; count--)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        PlayModeManager.Instance.UnpauseTracksAndNotes();

        // Resume game after countdown
        countdownUI.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
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

    #region GameModeTransitions
    // Method to deactivate all game modes
    private void DeactivateAllModes()
    {
        playAlongModePrefab.SetActive(false);
        tutorialModePrefab.SetActive(false);
        playModePrefab.SetActive(false);
    }

    // Method to start Free Play Mode
    public void StartFreePlayMode()
    {
        DeactivateAllModes();
        // Free play mode means deactivating all specific game modes, so no prefab is activated here
    }

    // Method to start Play Along Mode
    public void StartPlayAlongMode()
    {
        DeactivateAllModes();
        playAlongModePrefab.SetActive(true);
    }

    // Method to start Tutorial Mode
    public void StartTutorialMode()
    {
        DeactivateAllModes();
        tutorialModePrefab.SetActive(true);
    }

    // Method to start Play Mode
    public void StartPlayMode()
    {
        DeactivateAllModes();
        playModePrefab.SetActive(true);
    }
    #endregion

}
