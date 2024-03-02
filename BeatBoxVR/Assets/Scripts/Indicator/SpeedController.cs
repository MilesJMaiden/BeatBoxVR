using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public static SpeedController Instance;
    public float speed;
    void Start()
    {
        if (Instance == null)
            Instance = GetComponent<SpeedController>();
    }

   
}
