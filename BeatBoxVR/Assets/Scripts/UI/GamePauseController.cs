using System.Collections;
using UnityEngine;
using TMPro;

public class GamePauseController : MonoBehaviour
{
    public GameObject CountDownCanvas;
    public GameObject countdownUI; // Reference to countdown UI element
    //public AudioSource mainAudioSound; // pause audio

    public void PauseGame()
    {
        // Pause game logic
        Time.timeScale = 0;
        //mainAudioSound.Pause();
        CountDownCanvas.SetActive(false);
        // Show pause menu...
    }

    public void UnpauseGameWithCountdown()
    {
        CountDownCanvas.SetActive(true);
        StartCoroutine(CountdownAndUnpause());
    }

    private IEnumerator CountdownAndUnpause()
    {
        // Show countdown UI and wait
        countdownUI.SetActive(true);
        // Countdown logic...
        TMP_Text countText = countdownUI.GetComponent<TMP_Text>();
        
        countText.text = "3";
        yield return new WaitForSecondsRealtime(1); // 3-second countdown

        countText.text = "2";
        yield return new WaitForSecondsRealtime(1);

        countText.text = "1";
        yield return new WaitForSecondsRealtime(1);

        CountDownCanvas.SetActive(false);
        Time.timeScale = 1; // Resume game
        //mainAudioSound.Play();
    }
}
