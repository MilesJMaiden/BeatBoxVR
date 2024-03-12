using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChangeSpeed : MonoBehaviour
{
    public float speed;

    public string key;

    public void AssignSpeed()
    {
        TutorialSpeedController.Instance.speed = speed;

        NoteController.Instance.RestartBeat();
    }
}
