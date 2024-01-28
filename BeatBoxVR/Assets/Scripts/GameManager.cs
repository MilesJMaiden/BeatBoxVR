using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UIController uiController;
    private bool isGamePaused = false;

    // Call this method to toggle pause state
    public void TogglePauseGame()
    {
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
            Time.timeScale = 1;
            uiController.ToggleMenu(false);
        }
    }

    // Additional game management methods...
}
