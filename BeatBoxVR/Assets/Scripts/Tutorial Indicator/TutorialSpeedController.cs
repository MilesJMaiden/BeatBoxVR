using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpeedController : MonoBehaviour
{
    public static TutorialSpeedController Instance;
    public float speed;
    void Start()
    {
        if (Instance == null)
            Instance = GetComponent<TutorialSpeedController>();
    }

   
}
