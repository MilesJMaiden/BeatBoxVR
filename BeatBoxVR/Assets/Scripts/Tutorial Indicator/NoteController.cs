using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public static NoteController Instance;

    public TutorialIndicators hihat;
    public TutorialIndicators snare;
    public TutorialIndicators crash;
    public TutorialIndicators kick;
    public TutorialIndicators small;
    public TutorialIndicators medium;
    public TutorialIndicators floor;
    public TutorialIndicators ride;
    public TutorialIndicators splash;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<NoteController>();
        }
    }

    public void RestartBeat()
    {
        float nextPlayTime = Time.time + 5;

        hihat.nextPlayTime = nextPlayTime;
        snare.nextPlayTime = nextPlayTime;
        crash.nextPlayTime = nextPlayTime;
        kick.nextPlayTime = nextPlayTime;
        small.nextPlayTime = nextPlayTime;
        medium.nextPlayTime = nextPlayTime;
        floor.nextPlayTime = nextPlayTime;
        ride.nextPlayTime = nextPlayTime;
        splash.nextPlayTime = nextPlayTime;

        hihat.currentNote = 0;
        snare.currentNote = 0;
        crash.currentNote = 0;
        kick.currentNote = 0;
        small.currentNote = 0;
        medium.currentNote = 0;
        floor.currentNote = 0;
        ride.currentNote = 0;
        splash.currentNote = 0;



    }
}
