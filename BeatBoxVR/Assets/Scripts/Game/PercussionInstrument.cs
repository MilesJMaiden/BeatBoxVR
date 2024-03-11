using System.Collections;
using UnityEngine;

public class PercussionInstrument : MonoBehaviour
{
    public GameObject animationPivot;

    public GameObject smallSplashVFXPrefab;
    public GameObject mediumSplashVFXPrefab;
    public GameObject largeSplashVFXPrefab;

    public Collider surfaceCollider;
    public Collider rimCollider;
    public Animator parentAnimator;
    private SoundManager soundManager;

    public Transform centerPosition;

    public float drumBounceIntensity = 0.1f;
    public float cymbalRotationIntensity = 15.0f;
    public float animationDuration = 0.2f;
    public float vfxLifetime = 1.4f;

    private bool isAnimating = false;
    private bool animationsEnabled = true;

    public ScoreZone scoreZone;

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

                // Directly notify the ScoreZone of a hit on this instrument
                if (scoreZone != null)
                {
                    scoreZone.HandleInstrumentHit(this.tag);
                }
                else
                {
                    Debug.LogWarning("ScoreZone reference not set in PercussionInstrument.");
                }

                // Instantiate the VFX on drums if velocity is high enough
                if (velocity > 1)
                {
                    GameObject vfxPrefab = SelectVFXPrefabBasedOnVelocity(velocity);
                    if (vfxPrefab != null)
                    {
                        Vector3 spawnPosition = centerPosition.position + new Vector3(0, 0.1f, 0); // Adjusted to use centerPosition for VFX
                        InstantiateVFX(vfxPrefab, spawnPosition, Vector3.up); // Direction is up for consistency
                    }
                }
            }
        }
    }


    private void InstantiateVFX(GameObject vfxPrefab, Vector3 position, Vector3 direction)
    {
        Quaternion hitRotation = Quaternion.identity;
        GameObject vfxInstance = Instantiate(vfxPrefab, position, hitRotation);



        Destroy(vfxInstance, vfxLifetime);
        Debug.Log($"Instantiated VFX: {vfxPrefab.name} at position: {position}");
    }

    private GameObject SelectVFXPrefabBasedOnVelocity(float velocity)
    {
        if (velocity <= 4)
            return smallSplashVFXPrefab;
        else if (velocity <= 7)
            return mediumSplashVFXPrefab;
        else
            return largeSplashVFXPrefab;
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
}
