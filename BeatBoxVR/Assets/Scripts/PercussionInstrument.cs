using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercussionInstrument : MonoBehaviour
{
    public Collider surfaceCollider; // Surface collider
    public Collider rimCollider;     // Rim collider, if applicable (e.g., not used for cymbals)
    private SoundManager soundManager;

    public float drumBounceIntensity = 0.1f;
    public float cymbalRotationIntensity = 15.0f;
    public float animationDuration = 0.2f;

    private bool isAnimating = false;

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

            if (!isAnimating)
            {
                StartCoroutine(AnimateInstrument(velocity));
            }
            else
            {
                StopAllCoroutines(); // Stop current animation
                StartCoroutine(AnimateInstrument(velocity)); // Start a new animation
            }

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

    private IEnumerator AnimateInstrument(float velocity)
    {
        isAnimating = true;
        Vector3 originalPosition = transform.localPosition;
        Quaternion originalRotation = transform.localRotation;

        if (rimCollider != null) // Assuming drums have a rim collider
        {
            Vector3 targetPosition = originalPosition + Vector3.up * drumBounceIntensity * velocity;
            float elapsedTime = 0;

            while (elapsedTime < animationDuration)
            {
                transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPosition;
        }
        else
        {
            float rotationAmount = cymbalRotationIntensity * velocity;
            float elapsedTime = 0;

            while (elapsedTime < animationDuration)
            {
                transform.localRotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(0, 0, rotationAmount), elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = originalRotation;
        }

        isAnimating = false;
    }
}

