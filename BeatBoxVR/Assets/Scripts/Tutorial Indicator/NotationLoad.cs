using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotationLoad : MonoBehaviour
{
    public string hihat;
    public string snare;
    public string crash;
    public string kick;
    public string small;
    public string medium;
    public string floor;
    public string ride;
    public string splash;

    public string key;
    public void ChangeBeat()
    {
        NoteController.Instance.hihat.notation = hihat;
        NoteController.Instance.snare.notation = snare;
        NoteController.Instance.crash.notation = crash;
        NoteController.Instance.kick.notation = kick;
        NoteController.Instance.small.notation = small;
        NoteController.Instance.medium.notation = medium;
        NoteController.Instance.floor.notation = floor;
        NoteController.Instance.ride.notation = ride;
        NoteController.Instance.splash.notation = splash;

        NoteController.Instance.RestartBeat();

    }
}
