using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[System.Serializable]
public class SongData
{
    public AudioClip songClip;
    public float songSpeed;
    public AudioClip drumTrackClip;
}

public class PlayModeManager : MonoBehaviour
{
    public static PlayModeManager Instance;

    public PlayableDirector playableDirector; // Reference to the PlayableDirector component
    public TimelineAsset[] timelines; // Array of Timeline assets for different songs

    public float delayBeforeStart = 3.0f; // Delay before starting a song
    private int currentSongIndex = 0;

    public List<SongData> songsData = new List<SongData>();

    [Header("Notes Configuration")]
    public GameObject[] notePrefabs; // Prefabs for each note type
    public Transform[] noteSpawnPoints; // Spawn points for each note type

    [Header("Audio Playback")]
    public AudioSource audioSource;
    public AudioSource drumTrackSource;

    [Header("UI Components")]
    // Add UI components for displaying score and streak
    [Header("UI Components")]
    public TextMeshProUGUI scoreText; // Already existing
    public TextMeshProUGUI streakText; // Add this for streak display

    private int score = 0;
    private int streak = 0; // Declare the streak variable
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializePlayMode();
        UpdateScore(0);
    }

    void InitializePlayMode()
    {
        // Initial setup can be performed here if needed
    }

    // Starts the coroutine to switch songs after a specified delay
    public void SwitchToSongWithDelay(int songIndex)
    {
        StartCoroutine(SwitchSongAfterDelay(songIndex, delayBeforeStart));
    }

    private IEnumerator SwitchSongAfterDelay(int songIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (songIndex >= 0 && songIndex < timelines.Length && songIndex < songsData.Count)
        {
            currentSongIndex = songIndex;
            playableDirector.playableAsset = timelines[songIndex];
            playableDirector.Play();

            // Play the main song clip
            if (audioSource != null && songsData[songIndex].songClip != null)
            {
                audioSource.clip = songsData[songIndex].songClip;
                audioSource.Play();
            }

            // Play the additional drum track in sync
            if (drumTrackSource != null && songsData[songIndex].drumTrackClip != null)
            {
                drumTrackSource.clip = songsData[songIndex].drumTrackClip;
                drumTrackSource.Play();
            }
        }
        else
        {
            Debug.LogError("Song index out of range.");
        }
    }

    #region UI
    // Methods for UI buttons to control playback
    public void PauseSong()
    {
        playableDirector.Pause();
    }

    public void PlaySong()
    {
        if (playableDirector.state != PlayState.Playing)
            playableDirector.Play();
    }

    // Skips to the next song in the playlist
    public void SkipToNextSong()
    {
        currentSongIndex = (currentSongIndex + 1) % timelines.Length; // Increment and loop around if needed
        SwitchToSongWithDelay(currentSongIndex);
    }

    // UI button actions to switch to specific songs
    public void OnSong1ButtonPressed() { SwitchToSongWithDelay(0); }
    public void OnSong2ButtonPressed() { SwitchToSongWithDelay(1); }
    #endregion

    // Spawns notes of the specified types at their designated spawn points
    private void SpawnNoteOfType(int noteType)
    {
        if (noteType >= 0 && noteType < notePrefabs.Length && noteType < noteSpawnPoints.Length)
        {
            var spawnPoint = noteSpawnPoints[noteType];
            var noteInstance = Instantiate(notePrefabs[noteType], spawnPoint.position, spawnPoint.rotation); // Use spawnPoint's rotation
            var noteBlock = noteInstance.GetComponent<NoteBlock>();
            if (noteBlock != null)
            {
                noteBlock.InitializeNoteBlock(spawnPoint.forward, songsData[currentSongIndex].songSpeed);
            }
        }
    }

    // Updates score and streak display on the UI
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (streakText != null) streakText.text = "Streak: " + streak;
    }

    // Calculates the multiplier based on the current streak
    private int CalculateMultiplier()
    {
        if (streak >= 50) return 3;
        else if (streak >= 25) return 2;
        return 1;
    }

    // Method to update the score and potentially the streak
    public void UpdateScore(int pointsToAdd)
    {
        score += pointsToAdd * CalculateMultiplier(); // Use a multiplier based on the streak
        streak++; // Increment streak with each successful hit
        UpdateUI(); // Update the UI to reflect the new score and streak
    }

    // Method to be called at the start or end of a song to reset the score and streak
    public void ResetScoreAndStreak()
    {
        score = 0;
        streak = 0;
        UpdateUI();
    }

    public void IncrementStreak()
    {
        streak++;
        UpdateStreakDisplay(); // This method updates the streak UI
    }
    // Resets the streak to 0 and updates the UI

    public void ResetStreak()
    {
        streak = 0;
        UpdateUI();
    }

    private void UpdateStreakDisplay()
    {
        // Assuming you have a TextMeshProUGUI component for displaying the streak
        streakText.text = "Streak: " + streak;
    }

    //Individual Notes
    public void SpawnHiHatNote() { SpawnNoteOfType(0); }
    public void SpawnCrashNote() { SpawnNoteOfType(1); }
    public void SpawnSnareNote() { SpawnNoteOfType(2); }
    public void SpawnSmallTomNote() { SpawnNoteOfType(3); }
    public void SpawnKickDrumNote() { SpawnNoteOfType(4); }
    public void SpawnMediumNote() { SpawnNoteOfType(5); }
    public void SpawnFloorTomNote() { SpawnNoteOfType(6); }
    public void SpawnSplashNote() { SpawnNoteOfType(7); }
    public void SpawnRideNote() { SpawnNoteOfType(8); }
}