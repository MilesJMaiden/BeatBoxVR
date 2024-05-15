using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    //public GameObject drumKit; // Reference to the drum kit

    public static bool isGamePaused = false;

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

    //Audio Visualiser
    public GameObject avRing;
    public GameObject avBand;

    // Deactivate all game modes on start to ensure a clean slate
    private void Start()
    {
        DeactivateAllModes();
    }

    public void TogglePauseGame()
    {
        if (isGamePaused)
        {
            // Start the countdown and unpause logic
            StartCoroutine(CountdownAndUnpause());
        }
        else
        {
            // Pause the game immediately
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
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

        // Resume game after countdown
        countdownUI.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }

    #region GameModeTransitions
    // Method to deactivate all game modes
    private void DeactivateAllModes()
    {
        playAlongModePrefab.SetActive(false);
        tutorialModePrefab.SetActive(false);
        playModePrefab.SetActive(false);
    }

    //
    private void DeactivateAV()
    {
        // Audio Visualizer set false
        avRing.SetActive(false);
        avBand.SetActive(false);
    }

    // Method to start Free Play Mode
    public void StartFreePlayMode()
    {
        DeactivateAllModes();
        // Free play mode means deactivating all specific game modes, so no prefab is activated here

        avRing.SetActive(true);
        avBand.SetActive(true);
    }

    // Method to start Play Along Mode
    public void StartPlayAlongMode()
    {
        DeactivateAllModes();
        playAlongModePrefab.SetActive(true);

        avRing.SetActive(true);
        avBand.SetActive(true);
    }

    // Method to start Tutorial Mode
    public void StartTutorialMode()
    {
        DeactivateAllModes();
        tutorialModePrefab.SetActive(true);

        DeactivateAV();
    }

    // Method to start Play Mode
    public void StartPlayMode()
    {
        DeactivateAllModes();
        playModePrefab.SetActive(true);

        DeactivateAV();
        avBand.SetActive(true);
    }
    #endregion

}
