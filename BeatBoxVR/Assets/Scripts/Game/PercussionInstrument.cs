using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercussionInstrument : MonoBehaviour
{
    public GameObject mainParent;

    public Collider surfaceCollider; 
    public Collider rimCollider;
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
        if (other.CompareTag("LeftDrumstick") || other.CompareTag("RightDrumstick"))
        {
            float velocity = other.attachedRigidbody.velocity.magnitude;
            Debug.Log($"Percussion instrument hit detected. Instrument: {gameObject.name}, Velocity: {velocity}");

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
                string soundType = other == surfaceCollider ? "Surface" : "Rim";
                soundManager.PlaySound(this.tag + "Rim", transform.position, velocity);
                Debug.Log($"Played sound for: {this.tag + soundType}. Volume and Pitch influenced by Velocity: {velocity}");
            }
        }
    }

    //!This currently does not work! TODO 
    private IEnumerator AnimateInstrument(float velocity)
    {
        isAnimating = true;

        Transform targetTransform = mainParent ? mainParent.transform : transform;

        Vector3 originalPosition = targetTransform.localPosition;
        Quaternion originalRotation = targetTransform.localRotation;

        // Drum animation
        if (rimCollider != null) 
        {
            Vector3 targetPosition = originalPosition + Vector3.up * drumBounceIntensity * velocity;
            float elapsedTime = 0;

            while (elapsedTime < animationDuration)
            {
                targetTransform.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetTransform.localPosition = originalPosition;
        }
        // Cymbal animation
        else
        {
            float rotationAmount = cymbalRotationIntensity * velocity;
            float elapsedTime = 0;

            while (elapsedTime < animationDuration)
            {
                targetTransform.localRotation = Quaternion.Lerp(originalRotation, Quaternion.Euler(0, 0, rotationAmount), elapsedTime / animationDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetTransform.localRotation = originalRotation;
        }

        isAnimating = false;
    }
}

