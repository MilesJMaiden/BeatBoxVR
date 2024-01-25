using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PercussionInstrument : MonoBehaviour
{
    public Collider surfaceCollider; // Surface collider
    public Collider rimCollider;     // Rim collider, if applicable (e.g., not used for cymbals)
    private SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene");
        }

        if (surfaceCollider == null)
        {
            Debug.LogError("Surface collider not assigned on " + gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DrumstickTip") || other.CompareTag("Drumstick"))
        {
            float velocity = other.attachedRigidbody.velocity.magnitude;

            if (other == surfaceCollider)
            {
                soundManager.PlaySound(this.tag, transform.position, velocity);
            }
            else if (rimCollider != null && other == rimCollider)
            {
                soundManager.PlaySound(this.tag + "Rim", transform.position, velocity);
            }
        }
    }
}
