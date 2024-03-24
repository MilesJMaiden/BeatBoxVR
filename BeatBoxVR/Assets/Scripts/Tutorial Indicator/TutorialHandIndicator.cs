using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandIndicator : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject leftHand;

    public string expectedTag;
    public GameObject hitVFXPrefab; // VFX to instantiate on hit
    public GameObject missVFXPrefab; // VFX to instantiate on miss
    public float destroyDelay = 0.5f;
    public bool IsHit { get; private set; } = false;
    public void HandleHit()
    {
        if (!IsHit) // Check if the note has not already been hit
        {
            if (hitVFXPrefab != null)
            {
                GameObject hitVFXInstance = Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
                Destroy(hitVFXInstance, destroyDelay); // Destroy the hit VFX after the specified delay
            }

            // Correct instrument was hit

            Destroy(gameObject); // Destroy the note block itself after the delay
            IsHit = true; // Mark the note as hit
        }
    }
}
