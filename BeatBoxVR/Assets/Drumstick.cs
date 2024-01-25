using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drumstick : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float velocityThreshold = 1.0f; // Minimum velocity to play sound
    public SoundManager soundManager; // Reference to the SoundManager

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager reference not set on Drumstick");
        }
    }

    void Update()
    {
        // Optionally, implement logic to track velocity every 1 or 2 ms
    }

    void OnTriggerEnter(Collider other)
    {
        float velocity = rigidBody.velocity.magnitude;
        if (velocity > velocityThreshold)
        {
            soundManager.PlaySound(other.tag, velocity);
        }
    }
}
