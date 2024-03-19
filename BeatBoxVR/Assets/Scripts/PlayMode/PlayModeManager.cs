using System.Collections.Generic;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using System.Collections;

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
    private int currentSongIndex = -1;

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
        playableDirector.playOnAwake = false; // Prevent automatic timeline play
    }

    void Start()
    {
        InitializePlayMode();
        UpdateScore(0);
        colorOrigin = missZone.material.color;
    }

    void InitializePlayMode()
    {
        // Any initial setup
    }

    private void SwitchToSong(int songIndex)
    {
        if (songIndex < 0 || songIndex >= timelines.Length)
        {
            Debug.LogError("Song index out of range.");
            return;
        }

        // Destroy any existing note blocks and reset the score and streak
        DestroyAllNoteBlocks();
        ResetScoreAndStreak();

        // Setup and play the new song
        currentSongIndex = songIndex;
        playableDirector.playableAsset = timelines[songIndex];
        playableDirector.Play();
    }

    public void PauseTracksAndNotes()
    {
        audioSource.Pause();
        drumTrackSource.Pause();
        playableDirector.Pause();
    }

    public void UnpauseTracksAndNotes()
    {
        audioSource.UnPause();
        drumTrackSource.UnPause();
        playableDirector.Play();
    }

    public void DestroyAllNoteBlocks()
    {
        foreach (var noteBlock in FindObjectsOfType<NoteBlock>())
        {
            Destroy(noteBlock.gameObject);
        }
    }

    public void OnSongButtonPressed(int songIndex)
    {
        // Immediately switch to the selected song without delay
        SwitchToSong(songIndex);
    }


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
        // Change the miss zone's color to grey (or any color indicating a miss) temporarily.
        missZone.material.color = Color.grey;

        // Wait for a short period before reverting the color.
        yield return new WaitForSeconds(0.1f); // Adjust the duration based on how long you want the effect to last.

        // Revert the miss zone's color back to its original color.
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