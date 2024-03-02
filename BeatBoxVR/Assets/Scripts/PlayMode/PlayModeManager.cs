using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayModeManager : MonoBehaviour
{
    public PlayableDirector playableDirector; // Assign in the Inspector
    public TimelineAsset[] timelines;         // Assign your different song Timelines in the Inspector
    public float delayBeforeStart = 3.0f;     // Delay in seconds before a song starts

    // Start is called before the first frame update
    void Start()
    {
        InitializePlayMode();
    }

    void InitializePlayMode()
    {
        // Optionally, initialize your play mode with any necessary setup
    }

    // Public method to switch to a specific song with a delay
    public void SwitchToSongWithDelay(int songIndex)
    {
        StartCoroutine(SwitchSongAfterDelay(songIndex, delayBeforeStart));
    }

    // Coroutine to handle the delay before switching songs
    private IEnumerator SwitchSongAfterDelay(int songIndex, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (songIndex >= 0 && songIndex < timelines.Length)
        {
            playableDirector.playableAsset = timelines[songIndex];
            playableDirector.Play(); // Starts playing the selected timeline after the delay
        }
        else
        {
            Debug.LogError("Song index out of range: " + songIndex);
        }
    }

    // Example intermediary methods for UI buttons
    public void OnSong1ButtonPressed()
    {
        SwitchToSongWithDelay(0); // Index of the first song
    }

    public void OnSong2ButtonPressed()
    {
        SwitchToSongWithDelay(1); // Index of the second song
    }

    // Add more methods as needed for additional songs
}
