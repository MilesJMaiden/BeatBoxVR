using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[System.Serializable] // Make sure this is outside and above the PlayModeManager class
public class SongData
{
    public AudioClip songClip;
    public float songSpeed;
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

    // Coroutine that waits for a specified delay before switching to the selected song
    private IEnumerator SwitchSongAfterDelay(int songIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (songIndex >= 0 && songIndex < timelines.Length && songIndex < songsData.Count)
        {
            currentSongIndex = songIndex;
            playableDirector.playableAsset = timelines[songIndex];
            playableDirector.Play();

            if (audioSource != null && songsData[songIndex].songClip != null)
            {
                audioSource.clip = songsData[songIndex].songClip;
                audioSource.Play();
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
            var noteInstance = Instantiate(notePrefabs[noteType], noteSpawnPoints[noteType].position, Quaternion.identity);
            var noteBlock = noteInstance.GetComponent<NoteBlock>();
            if (noteBlock != null)
            {
                noteBlock.InitializeNoteBlock(songsData[currentSongIndex].songSpeed);
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

    #region Notes
    #region DoubleNotes
    //DoubleNotes - 36
    //public void SpawnHiHatCrashNotes() 
    //{ 
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(1);
    //}
    //public void SpawnHiHatSnareNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(2);
    //}
    //public void SpawnHiHatSmallTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(3);
    //}
    //public void SpawnHiHatKickDrumNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //}
    //public void SpawnHiHatMediumTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnHiHatFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnCrashSnareNotes()
    //{
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(2);
    //}
    //public void SpawnCrashSmallTomNotes()
    //{
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(3);
    //}
    //public void SpawnCrashKickDrumNotes()
    //{
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(4);
    //}
    //public void SpawnCrashMediumTomNotes()
    //{
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnCrashFloorTomNotes()
    //{
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnCrashSplashNotes()
    //{
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnCrashRideNotes()
    //{
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnSnareSmallTomNotes()
    //{
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(3);
    //}
    //public void SpawnSnareKickDrumNotes()
    //{
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(4);
    //}
    //public void SpawnSnareMediumTomNotes()
    //{
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnSnareFloorTomNotes()
    //{
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnSnareSplashNotes()
    //{
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnSnareRideNotes()
    //{
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnSmallTomKickDrumNotes()
    //{
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(4);
    //}
    //public void SpawnSmallTomMediumTomNotes()
    //{
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnSmallTomFloorTomNotes()
    //{
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnSmallTomSplashNotes()
    //{
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnSmallTomRideNotes()
    //{
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnKickDrumMediumTomNotes()
    //{
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnKickDrumFloorTomNotes()
    //{
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnKickDrumSplashNotes()
    //{
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnKickDrumRideNotes()
    //{
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnMediumTomFloorTomNotes()
    //{
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnMediumTomSplashNotes()
    //{
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnMediumTomRideNotes()
    //{
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnFloorTomSplashNotes()
    //{
    //    SpawnNoteOfType(6);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnFloorTomRideNotes()
    //{
    //    SpawnNoteOfType(6);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnSplashRideNotes()
    //{
    //    SpawnNoteOfType(7);
    //    SpawnNoteOfType(8);
    //}
    //#endregion

    //#region TripleNotes
    ////Triple Notes - 28
    //public void SpawnHiHatCrashSnareNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(2);
    //}
    //public void SpawnHiHatCrashSmallTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(3);
    //}
    //public void SpawnHiHatCrashKickDrumNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(4);
    //}
    //public void SpawnHiHatCrashMediumTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnHiHatCrashFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatCrashSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatCrashRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatSnareSmallTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(3);
    //}
    //public void SpawnHiHatSnareKickDrumNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(4);
    //}
    //public void SpawnHiHatSnareMediumTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnHiHatSnareFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatSnareSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatSnareRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatSmallTomKickDrumNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(4);
    //}
    //public void SpawnHiHatSmallTomMediumTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnHiHatSmallTomFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatSmallTomSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatSmallTomRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatKickDrumMediumTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnHiHatKickDrumFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatKickDrumSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatKickDrumRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatMediumTomFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatMediumTomSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatMediumTomRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatFloorTomSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(6);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatFloorTomRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(6);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatSplashRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(7);
    //    SpawnNoteOfType(8);
    //}
    //#endregion

    //#region QuadNotes
    ////Quad Notes
    //public void SpawnHiHatKickDrumCrashSnareNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(2);
    //}
    //public void SpawnHiHatKickDrumCrashSmallTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(3);
    //}
    //public void SpawnHiHatKickDrumCrashMediumTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnHiHatKickDrumCrashFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatKickDrumCrashSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatKickDrumCrashRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(1);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatKickDrumSnareSmallTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(3);
    //}
    //public void SpawnHiHatKickDrumSnareMediumTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(5);

    //}
    //public void SpawnHiHatKickDrumSnareFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatKickDrumSnareSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatKickDrumSnareRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(2);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatKickDrumSmallTomMediumTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(5);
    //}
    //public void SpawnHiHatKickDrumSmallTomFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatKickDrumSmallTomSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatKickDrumSmallTomRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(3);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatKickDrumMediumTomFloorTomNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(6);
    //}
    //public void SpawnHiHatKickDrumMediumTomSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatKickDrumMediumTomRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(5);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatKickDrumFloorTomSplashNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(6);
    //    SpawnNoteOfType(7);
    //}
    //public void SpawnHiHatKickDrumFloorTomRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(6);
    //    SpawnNoteOfType(8);
    //}

    //public void SpawnHiHatKickDrumSplashRideNotes()
    //{
    //    SpawnNoteOfType(0);
    //    SpawnNoteOfType(4);
    //    SpawnNoteOfType(7);
    //    SpawnNoteOfType(8);
    //}
    #endregion
    #endregion Notes End
}