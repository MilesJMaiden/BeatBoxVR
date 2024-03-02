using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayModeManager : MonoBehaviour
{
    public PlayableDirector playableDirector; // Assign in the Inspector
    public TimelineAsset[] timelines;         // Assign your different song Timelines in the Inspector
    public float delayBeforeStart = 3.0f;     // Delay in seconds before a song starts
    public GameObject[] notePrefabs;          // Assign note prefabs in the Inspector

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

    // Method to spawn notes based on rules
    public void SpawnNotes(int[] noteTypes)
    {
        bool includesHiHat = System.Array.IndexOf(noteTypes, 0) != -1; // Check if HiHat is included
        bool includesKickDrum = System.Array.IndexOf(noteTypes, 4) != -1; // Check if KickDrum is included

        // Calculate max notes allowed
        int maxNotes = 2 + (includesHiHat ? 1 : 0) + (includesKickDrum ? 1 : 0);

        // Spawn notes up to the max allowed
        for (int i = 0; i < Mathf.Min(noteTypes.Length, maxNotes); i++)
        {
            SpawnNoteOfType(noteTypes[i]);
        }
    }

    private void SpawnNoteOfType(int noteType)
    {
        if (noteType >= 0 && noteType < notePrefabs.Length)
        {
            // Instantiate note at a specific location. Modify as needed.
            Instantiate(notePrefabs[noteType], new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    public void SpawnHiHatNote() { SpawnNoteOfType(0); }
    public void SpawnCrashNote() { SpawnNoteOfType(1); }
    public void SpawnSnareNote() { SpawnNoteOfType(2); }
    public void SpawnSmallTomNote() { SpawnNoteOfType(3); }
    public void SpawnKickDrumNote() { SpawnNoteOfType(4); }
    public void SpawnMediumNote() { SpawnNoteOfType(5); }
    public void SpawnFloorTomNote() { SpawnNoteOfType(6); }
    public void SpawnSplashNote() { SpawnNoteOfType(7); }
    public void SpawnRidehNote() { SpawnNoteOfType(8); }

    public void SpawnNoteCombination(int[] noteTypes)
    {
        SpawnNotes(noteTypes);
    }

}
