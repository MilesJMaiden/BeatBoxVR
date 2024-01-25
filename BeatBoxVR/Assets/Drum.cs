using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour
{
    private SoundManager soundManager;

    void Start()
    {
        // Find the SoundManager in the scene
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DrumstickTip") || other.CompareTag("Drumstick"))
        {
            // Calculate velocity
            float velocity = other.attachedRigidbody.velocity.magnitude;

            // Play sound based on the tag of this drum/cymbal part
            soundManager.PlaySound(gameObject.tag, transform.position, velocity);
        }
    }
}
