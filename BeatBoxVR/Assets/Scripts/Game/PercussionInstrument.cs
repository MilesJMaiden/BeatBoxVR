using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PercussionInstrument : MonoBehaviour
{
    //
    private Player player;

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

    private bool vfxEnabled = true;
    private bool animationsEnabled = true;
    
    public ScoreZone scoreZone;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

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

    public void ToggleAnimations()
    {
        animationsEnabled = !animationsEnabled;
        Debug.Log("Animations toggled. Now: " + animationsEnabled);
    }

    public void ToggleVFX() // Method to toggle VFX instantiation
    {
        vfxEnabled = !vfxEnabled;
        Debug.Log("VFX toggled. Now: " + vfxEnabled);
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Drumstick"))
    //    {
    //        Drumstick drumstick = other.GetComponent<Drumstick>();
    //        if (drumstick != null)
    //        {
    //            float velocity = drumstick.GetCurrentVelocity();
    //            //Debug.Log($"Percussion instrument hit detected. Instrument: {gameObject.name}, Velocity: {velocity}");

    //            // Conditional VFX instantiation based on the vfxEnabled toggle
    //            if (vfxEnabled && velocity > 1)
    //            {
    //                GameObject vfxPrefab = SelectVFXPrefabBasedOnVelocity(velocity);
    //                if (vfxPrefab != null)
    //                {
    //                    //Vector3 spawnPosition = centerPosition.position + new Vector3(0, 0.1f, 0);
    //                    //InstantiateVFX(vfxPrefab, spawnPosition, Vector3.up, velocity);
    //                }
    //            }

    //            bool isHiHatOpen = (this.tag == "HiHat") ? player.GetIsHiHatOpen() : false;
    //            PlayInstrumentSound(other, velocity);

    //            // Conditional animation based on the animationsEnabled toggle
    //            if (animationsEnabled && (!isAnimating || velocity > drumstick.LastHitVelocity))
    //            {
    //                StartCoroutine(AnimateInstrument());
    //            }
    //        }
    //    }
    //}

    public Vector3 VFXPosition(Vector3 spawnPosition)
    {
        return spawnPosition = centerPosition.position + new Vector3(0, 0.1f, 0);
    }
    private void InstantiateVFX(GameObject vfxPrefab, Vector3 position, Vector3 direction, float velocity)
    {
        // Instantiate vfx
        Quaternion hitRotation = Quaternion.identity;
        GameObject vfxInstance = Instantiate(vfxPrefab, position, hitRotation);

        // Adjust vfx size by hit velocity
        float scaleMultiplier = CalculateScaleMultiplier(velocity);
        ApplyScaleMultiplierInChild(vfxInstance, scaleMultiplier);

        // Destroy vfx after life duration
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

    private float CalculateScaleMultiplier(float velocity)
    {
        return Mathf.Clamp(velocity, 0.1f, 1.8f);
    }

    private void ApplyScaleMultiplierInChild(GameObject vfxInstance, float scaleMultiplier)
    {
        for(int i = 0; i < vfxInstance.transform.childCount; i++)
        {
            Transform child = vfxInstance.transform.GetChild(i);
            child.localScale = new Vector3(0.2f, 0.2f, 0.2f * scaleMultiplier);
        }
    }


    private void PlayInstrumentSound(Collider other, float velocity)
    {
        bool isHiHatOpen = player.GetIsHiHatOpen(); // Ensure this accesses the current state correctly.
        string soundType = (other == rimCollider && rimCollider != null) ? " Rim" : "";
        // Append the sound type based on hi-hat state for hi-hat tags
        string tagToUse = this.tag + (this.tag == "HiHat" ? (isHiHatOpen ? " Open" : " Closed") : "");
        soundManager.PlaySound(tagToUse, transform.position, velocity, isHiHatOpen);
    }

    public IEnumerator AnimateInstrument()
    {
        if (!animationsEnabled) yield break;

        isAnimating = true;
        Debug.Log("Animating Instrument: " + gameObject.name + " Animation Enabled: " + animationsEnabled);
        parentAnimator.SetBool("isAnimating", true);

        yield return new WaitForSeconds(animationDuration);

        if (animationsEnabled)
        {
            isAnimating = false;
            Debug.Log("Stopping Animation: " + gameObject.name + " Animation Enabled: " + animationsEnabled);
            parentAnimator.SetBool("isAnimating", false);
        }
    }
}
