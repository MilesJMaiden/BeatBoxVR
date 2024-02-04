using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIController uiController;
    private bool isGamePaused = false;

    public GameObject countDownCanvas;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePauseGame();
        }
    }

    // Call this method to toggle pause state
    public void TogglePauseGame()
    {
        //if (!uiController.mainMenuCanvasGroup.gameObject.activeSelf)
        //{
        //    Time.timeScale = 0;
        //    uiController.ToggleMenu(true);
        //}

        // if countDownCanvas is activate, do not run the codes below.
        if (countDownCanvas.activeSelf)
        {
            return;
        }

        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            // Pause logic
            Time.timeScale = 0;
            uiController.ToggleMenu(true);
        }
        else
        {
            // Unpause logic
            //Time.timeScale = 1;
            uiController.ToggleMenu(false);
            uiController.GetComponent<GamePauseController>().UnpauseGameWithCountdown();
        }
    }

    // Additional game management methods...
}
