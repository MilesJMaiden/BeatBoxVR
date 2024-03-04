using System.Collections;
using UnityEngine;

public class PercussionInstrument : MonoBehaviour
{
    public GameObject animationPivot;
    public Collider surfaceCollider;
    public Collider rimCollider;
    public Animator parentAnimator;
    private SoundManager soundManager;

    public float drumBounceIntensity = 0.1f;
    public float cymbalRotationIntensity = 15.0f;
    public float animationDuration = 0.2f;

    private bool isAnimating = false;
    private bool animationsEnabled = true;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        parentAnimator.SetBool("isAnimating", isAnimating);
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene");
        }


        //removed animationPivot == null
        if (surfaceCollider == null)
        {
            Debug.LogError("Required components not assigned on " + gameObject.name + " surfaceCollider: " + surfaceCollider.ToString()
                + " animationPivot: " + animationPivot.ToString());

        }
    }

    public void ToggleAnimations(bool enable) // Method to toggle animations on/off
    {
        animationsEnabled = enable;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if a drumstick hits the instrument
        if (other.CompareTag("RightDrumstick") || other.CompareTag("LeftDrumstick"))
        {
            Drumstick drumstick = other.GetComponent<Drumstick>();
            if (drumstick != null && animationsEnabled)
            {
                float velocity = drumstick.GetCurrentVelocity();
                Debug.Log($"Percussion instrument hit detected. Instrument: {gameObject.name}, Velocity: {velocity}");

                // Perform animations if enabled
                if (!isAnimating || velocity > drumstick.LastHitVelocity)
                {
                    StartCoroutine(AnimateInstrument());
                }

                // Play sound based on which part of the instrument was hit
                PlayInstrumentSound(other, velocity);

                // Notify the nearest NoteBlock of a hit
                NotifyNoteBlockOfHit();
            }
        }
    }

    private void NotifyNoteBlockOfHit()
    {
        // Find the nearest NoteBlock and check if its tag matches this instrument's tag
        NoteBlock[] noteBlocks = FindObjectsOfType<NoteBlock>();
        foreach (var noteBlock in noteBlocks)
        {
            if (noteBlock.expectedTag == this.tag && Vector3.Distance(transform.position, noteBlock.transform.position) < 1f) // Assuming a small threshold for distance
            {
                noteBlock.HandleHit();
                break; // Assuming only one NoteBlock can be relevant at a time
            }
        }
    }

    private void PlayInstrumentSound(Collider other, float velocity)
    {
        if (other == surfaceCollider || (rimCollider != null && other == rimCollider))
        {
            string soundType = other == surfaceCollider ? "" : "Rim";
            soundManager.PlaySound(this.tag + soundType, transform.position, velocity);
        }
    }

    public IEnumerator AnimateInstrument()
    {
        
        isAnimating = true;
        Debug.Log("isAnimating" + isAnimating);
        parentAnimator.SetBool("isAnimating", isAnimating);

        yield return new WaitForSeconds(animationDuration);
        
        isAnimating = false;
        parentAnimator.SetBool("isAnimating", isAnimating);

    }  

    /* Joshua IEnum animation - NOT USED
    private IEnumerator AnimateInstrument(float velocity)
    {
        isAnimating = true;
        Transform targetTransform = animationPivot.transform;

        bool isDrum = rimCollider != null;

        if (isDrum)
        {
            Vector3 bounceDirection = Vector3.up * drumBounceIntensity * velocity;
            yield return StartCoroutine(AnimateBounce(targetTransform, bounceDirection));
        }
        else
        {
            yield return StartCoroutine(AnimateRotation(targetTransform, velocity));
        }

        isAnimating = false;
    }

    private IEnumerator AnimateBounce(Transform target, Vector3 direction)
    {
        Vector3 originalPosition = target.localPosition;
        Vector3 targetPosition = originalPosition + direction;

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            target.localPosition = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localPosition = originalPosition;
    }

    private IEnumerator AnimateRotation(Transform target, float velocity)
    {
        Quaternion originalRotation = target.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, cymbalRotationIntensity * velocity);

        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            target.localRotation = Quaternion.Lerp(originalRotation, targetRotation, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localRotation = originalRotation;
    }
    */
}
