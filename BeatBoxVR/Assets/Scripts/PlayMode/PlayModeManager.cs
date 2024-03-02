using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayModeManager : MonoBehaviour
{
    public PlayableDirector playableDirector; // Assign in the Inspector
    public TimelineAsset[] timelines;         // Assign your different song Timelines in the Inspector
    public float delayBeforeStart = 3.0f;     // Delay in seconds before a song starts

    [Header("Notes Configuration")]
    public GameObject[] notePrefabs; // Prefabs for each note type
    public Transform[] noteSpawnPoints; // Spawn points for each note type

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

    public void SpawnNoteOfType(params int[] noteTypes)
    {
        foreach (int noteType in noteTypes)
        {
            if (noteType >= 0 && noteType < notePrefabs.Length && noteType < noteSpawnPoints.Length)
            {
                Instantiate(notePrefabs[noteType], noteSpawnPoints[noteType].position, Quaternion.identity);
            }
        }
    }

    #region Notes
    #region Individual Notes
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
    #endregion

    #region DoubleNotes
    //DoubleNotes
    public void SpawnHiHatCrashNotes() { 
        
        SpawnNoteOfType(0);
        SpawnNoteOfType(1);

    }
    public void SpawnHiHatSnarehNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(2);

    }
    public void SpawnHiHatSmallTomhNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(3);

    }
    public void SpawnHiHatKickDrumhNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);

    }
    public void SpawnHiHatMediumTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(5);

    }
    public void SpawnHiHatFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatRidehNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(8);

    }

    public void SpawnCrashSnareNotes()
    {

        SpawnNoteOfType(1);
        SpawnNoteOfType(2);

    }
    public void SpawnCrashSmallTomNotes()
    {

        SpawnNoteOfType(1);
        SpawnNoteOfType(3);

    }
    public void SpawnCrashKickDrumNotes()
    {

        SpawnNoteOfType(1);
        SpawnNoteOfType(4);

    }
    public void SpawnCrashMediumTomNotes()
    {

        SpawnNoteOfType(1);
        SpawnNoteOfType(5);

    }
    public void SpawnCrashFloorTomNotes()
    {

        SpawnNoteOfType(1);
        SpawnNoteOfType(6);

    }
    public void SpawnCrashSplashNotes()
    {

        SpawnNoteOfType(1);
        SpawnNoteOfType(7);

    }
    public void SpawnCrashRideNotes()
    {

        SpawnNoteOfType(1);
        SpawnNoteOfType(8);

    }

    public void SpawnSnareSmallTomNotes()
    {

        SpawnNoteOfType(2);
        SpawnNoteOfType(3);

    }
    public void SpawnSnareKickDrumTomNotes()
    {

        SpawnNoteOfType(2);
        SpawnNoteOfType(4);

    }
    public void SpawnSnareMediumTomNotes()
    {

        SpawnNoteOfType(2);
        SpawnNoteOfType(5);

    }
    public void SpawnSnareFloorTomNotes()
    {

        SpawnNoteOfType(2);
        SpawnNoteOfType(6);

    }
    public void SpawnSnareSplashNotes()
    {

        SpawnNoteOfType(2);
        SpawnNoteOfType(7);

    }
    public void SpawnSnareRideNotes()
    {

        SpawnNoteOfType(2);
        SpawnNoteOfType(8);

    }

    public void SpawnSmallTomKickDrumNotes()
    {

        SpawnNoteOfType(3);
        SpawnNoteOfType(4);

    }
    public void SpawnSmallTomMediumTomNotes()
    {

        SpawnNoteOfType(3);
        SpawnNoteOfType(5);

    }
    public void SpawnSmallTomFloorTomNotes()
    {

        SpawnNoteOfType(3);
        SpawnNoteOfType(6);

    }
    public void SpawnSmallTomSplashNotes()
    {

        SpawnNoteOfType(3);
        SpawnNoteOfType(7);

    }
    public void SpawnSmallTomRideNotes()
    {

        SpawnNoteOfType(3);
        SpawnNoteOfType(8);

    }

    public void SpawnKickDrumMediumTomNotes()
    {

        SpawnNoteOfType(4);
        SpawnNoteOfType(5);

    }
    public void SpawnKickDrumFloorTomNotes()
    {

        SpawnNoteOfType(4);
        SpawnNoteOfType(6);

    }
    public void SpawnKickDrumSplashNotes()
    {

        SpawnNoteOfType(4);
        SpawnNoteOfType(7);

    }
    public void SpawnKickDrumRideNotes()
    {

        SpawnNoteOfType(4);
        SpawnNoteOfType(8);

    }

    public void SpawnMediumTomFloorTomNotes()
    {

        SpawnNoteOfType(5);
        SpawnNoteOfType(6);

    }
    public void SpawnMediumTomSplashNotes()
    {

        SpawnNoteOfType(5);
        SpawnNoteOfType(7);

    }
    public void SpawnMediumTomRideNotes()
    {

        SpawnNoteOfType(5);
        SpawnNoteOfType(8);

    }

    public void SpawnFloorTomSplashNotes()
    {

        SpawnNoteOfType(6);
        SpawnNoteOfType(7);

    }
    public void SpawnFloorTomRideNotes()
    {

        SpawnNoteOfType(6);
        SpawnNoteOfType(8);

    }

    public void SpawnSplashRideNotes()
    {

        SpawnNoteOfType(7);
        SpawnNoteOfType(8);

    }
    #endregion

    #region TripleNotes
    //Triple Notes
    public void SpawnHiHatCrashSnareNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(1);
        SpawnNoteOfType(2);

    }
    public void SpawnHiHatCrashSmallTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(1);
        SpawnNoteOfType(3);

    }
    public void SpawnHiHatCrashKickDrumNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(1);
        SpawnNoteOfType(4);

    }
    public void SpawnHiHatCrashMediumTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(1);
        SpawnNoteOfType(5);

    }
    public void SpawnHiHatCrashFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(1);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatCrashSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(1);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatCrashRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(1);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatSnareSmallTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(2);
        SpawnNoteOfType(3);

    }
    public void SpawnHiHatSnareKickDrumNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(2);
        SpawnNoteOfType(4);

    }
    public void SpawnHiHatSnareMediumTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(2);
        SpawnNoteOfType(5);

    }
    public void SpawnHiHatSnareFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(2);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatSnareSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(2);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatSnareRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(2);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatSmallTomKickDrumNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(3);
        SpawnNoteOfType(4);

    }
    public void SpawnHiHatSmallTomMediumTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(3);
        SpawnNoteOfType(5);

    }
    public void SpawnHiHatSmallTomFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(3);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatSmallTomSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(3);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatSmallTomRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(3);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatKickDrumMediumTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(5);

    }
    public void SpawnHiHatKickDrumFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatKickDrumSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatKickDrumRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatMediumTomFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(5);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatMediumTomSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(5);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatMediumTomRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(5);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatFloorTomSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(6);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatFloorTomRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(6);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatSplashRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(7);
        SpawnNoteOfType(8);

    }
    #endregion

    #region QuadNotes
    //Quad Notes
    public void SpawnHiHatKickDrumCrashSnareNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(1);
        SpawnNoteOfType(2);

    }
    public void SpawnHiHatKickDrumCrashSmallTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(1);
        SpawnNoteOfType(3);

    }
    public void SpawnHiHatKickDrumCrashMediumTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(1);
        SpawnNoteOfType(5);

    }
    public void SpawnHiHatKickDrumCrashFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(1);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatKickDrumCrashSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(1);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatKickDrumCrashRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(1);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatKickDrumSnareSmallTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(2);
        SpawnNoteOfType(3);

    }
    public void SpawnHiHatKickDrumSnareMediumTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(2);
        SpawnNoteOfType(5);

    }
    public void SpawnHiHatKickDrumSnareFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(2);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatKickDrumSnareSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(2);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatKickDrumSnareRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(2);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatKickDrumSmallTomMediumTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(3);
        SpawnNoteOfType(5);

    }
    public void SpawnHiHatKickDrumSmallTomFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(3);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatKickDrumSmallTomSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(3);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatKickDrumSmallTomRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(3);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatKickDrumMediumTomFloorTomNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(5);
        SpawnNoteOfType(6);

    }
    public void SpawnHiHatKickDrumMediumTomSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(5);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatKickDrumMediumTomRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(5);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatKickDrumFloorTomSplashNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(6);
        SpawnNoteOfType(7);

    }
    public void SpawnHiHatKickDrumFloorTomRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(6);
        SpawnNoteOfType(8);

    }

    public void SpawnHiHatKickDrumSplashRideNotes()
    {

        SpawnNoteOfType(0);
        SpawnNoteOfType(4);
        SpawnNoteOfType(7);
        SpawnNoteOfType(8);

    }
    #endregion

    #endregion Notes End

    public void SpawnNoteCombination(int[] noteTypes)
    {
        SpawnNotes(noteTypes);
    }

}
