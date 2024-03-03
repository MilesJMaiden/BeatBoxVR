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
    private bool animationsEnabled = true; // New bool to enable/disable animations

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

        Drumstick drumstick = other.GetComponent<Drumstick>();
        if (drumstick != null && animationsEnabled)
        {
            float velocity = drumstick.GetCurrentVelocity();
            NoteBlock noteBlock = FindObjectOfType<NoteBlock>();
            Debug.Log($"Percussion instrument hit detected. Instrument: {gameObject.name}, Velocity: {velocity}");

            if (!isAnimating || velocity > drumstick.LastHitVelocity) // Prioritize higher velocity hits
            {
                /*
                StopAllCoroutines(); // Ensure a smooth transition between animations
                StartCoroutine(AnimateInstrument(velocity));
                */
                StartCoroutine(AnimateInstrument());

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
