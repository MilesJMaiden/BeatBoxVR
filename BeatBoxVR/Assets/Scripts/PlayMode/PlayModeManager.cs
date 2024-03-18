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

    public PlayableDirector playableDirector;
    public TimelineAsset[] timelines;

    public float delayBeforeStart = 3.0f; // Delay before starting a song
    private float initialDelay = 4.5f; // Additional delay for synchronization
    private int currentSongIndex = -1; // Initialized to -1 to indicate no song is selected


    public List<SongData> songsData = new List<SongData>();

    [Header("Notes Configuration")]
    public GameObject[] notePrefabs;
    public Transform[] noteSpawnPoints;

    [Header("Audio Playback")]
    public AudioSource audioSource;
    public AudioSource drumTrackSource;
    public AudioSource soundEffectSource;

    [Header("UI Components")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI streakText;

    [Header("MissHit")]
    public MeshRenderer missZone;
    private Color colorOrigin;
    private int score = 0;
    private int streak = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // Make sure the timeline doesn't play automatically.
        playableDirector.playOnAwake = false;
    }

    void Start()
    {
        InitializePlayMode();
        UpdateScore(0);

        colorOrigin = missZone.material.color;
    }

    void InitializePlayMode()
    {
        // Initial setup can be performed here if needed
    }

    public void SwitchToSongWithDelay(int songIndex)
    {
        StartCoroutine(SwitchSongAfterDelays(songIndex, delayBeforeStart + initialDelay));
    }

    private IEnumerator SwitchSongAfterDelays(int songIndex, float totalDelay)
    {
        yield return new WaitForSeconds(totalDelay);
        if (songIndex >= 0 && songIndex < timelines.Length && songIndex < songsData.Count)
        {
            currentSongIndex = songIndex;
            playableDirector.playableAsset = timelines[songIndex];

            // Delay the start of the song and drum tracks by the initial delay
           StartCoroutine(StartTracksWithDelay(songIndex, initialDelay));
        }
        else
        {
            Debug.LogError("Song index out of range.");
        }
    }

    public void SwitchToSong(int songIndex)
    {
        // Ensure we're operating within bounds.
        if (songIndex < 0 || songIndex >= timelines.Length)
        {
            Debug.LogError("Song index out of range.");
            return;
        }

        // Destroy any existing note blocks.
        DestroyAllNoteBlocks();

        // Set the current song index.
        currentSongIndex = songIndex;

        // Assign the selected timeline.
        playableDirector.playableAsset = timelines[songIndex];

        // Delay the start of tracks to ensure synchronization.
        StartCoroutine(StartTracksWithDelay(songIndex, initialDelay));
    }

    private IEnumerator StartTracksWithDelay(int songIndex, float delay)
    {
        // Wait for the specified delay.
        yield return new WaitForSeconds(delay);

        // Start playing the selected tracks.
        if (audioSource && songsData[songIndex].songClip)
        {
            audioSource.clip = songsData[songIndex].songClip;
            audioSource.Play();
        }

        if (drumTrackSource && songsData[songIndex].drumTrackClip)
        {
            drumTrackSource.clip = songsData[songIndex].drumTrackClip;
            drumTrackSource.Play();
        }

        // Start the timeline.
        playableDirector.Play();
    }

    public void PauseTracksAndNotes()
    {
        // Pause the audio sources
        if (audioSource != null) audioSource.Pause();
        if (drumTrackSource != null) drumTrackSource.Pause();

        // Pause the timeline (which should control the movement of notes)
        playableDirector.Pause();
    }

    public void UnpauseTracksAndNotes(bool isPaused)
    {
        if (!isPaused)
        {
            // Unpause the audio sources
            if (audioSource != null) audioSource.UnPause();
            if (drumTrackSource != null) drumTrackSource.UnPause();

            // Resume the timeline
            playableDirector.Play();
        }
        else
        {
            // Logic to pause (if needed)
        }
    }

    public void DestroyAllNoteBlocks()
    {
        foreach (var noteBlock in FindObjectsOfType<NoteBlock>())
        {
            Destroy(noteBlock.gameObject);
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

    // UI button actions to switch to specific songs
    public void OnSong1ButtonPressed() {

        SwitchToSongWithDelay(0);
        DestroyAllNoteBlocks();
        ResetScoreAndStreak();
    }
    public void OnSong2ButtonPressed() {
        SwitchToSongWithDelay(1);
        DestroyAllNoteBlocks();
        ResetScoreAndStreak();
    }
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
        if (streakText != null) streakText.text = "Streak: x " + streak;
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

    public void ResetStreak()
    {
        streak = 0;
        UpdateUI();
    }

    private void UpdateStreakDisplay()
    {
        // Assuming you have a TextMeshProUGUI component for displaying the streak
        streakText.text = "x " + streak;
    }

    public void PlayMissHitSound()
    {
        soundEffectSource.Play();
    }

    public IEnumerator MissZoneGetLit()
    {
        missZone.material.color = Color.grey;
        yield return new WaitForSeconds(0.1f);

        missZone.material.color = colorOrigin;
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