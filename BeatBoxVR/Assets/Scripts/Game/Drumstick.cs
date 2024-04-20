using UnityEngine;
using UnityEngine.XR;

public class Drumstick : MonoBehaviour
{
    private Player player;

    private SoundManager soundManager;
    
    public GameObject whiteSparkVFXPrefab;
    public GameObject yellowSparkVFXPrefab;
    public GameObject redSparkVFXPrefab;
    
    public float vfxLifetime = 1.4f;
    private const float MaxVelocity = 10f;

    public Transform tipTransform;
    private Vector3 previousTipPosition;
    private Vector3 tipMovementDirection;
    private float tipVelocity;

    public bool instantiateVFX = false; // Flag to control VFX instantiation
    public bool enableHapticFeedback = true; // Flag to control haptic feedback
    public float LastHitVelocity { get; private set; }

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene");
        }

        previousTipPosition = tipTransform.position;
    }

    void Update()
    {
        Vector3 currentTipPosition = tipTransform.position;
        tipMovementDirection = currentTipPosition - previousTipPosition;
        tipVelocity = tipMovementDirection.magnitude / Time.deltaTime;
        previousTipPosition = currentTipPosition;
    }

    // Method to get current velocity
    public float GetCurrentVelocity()
    {
        return Mathf.Clamp(tipVelocity, 0, MaxVelocity);
    }


    void OnTriggerEnter(Collider other)
    {
        if (tipMovementDirection.y < 0) // Check if moving downwards
        {
            float clampedVelocity = GetCurrentVelocity();
            Debug.Log($"Drumstick hit detected. Velocity: {clampedVelocity}. Collider Tag: {other.tag}");

            bool isHiHatOpen = player?.GetIsHiHatOpen() ?? false;
            soundManager.PlaySound(other.tag, tipTransform.position, clampedVelocity / MaxVelocity, isHiHatOpen);

            if (clampedVelocity > 1 && instantiateVFX)
            {
                GameObject vfxPrefab = SelectVFXPrefabBasedOnVelocity(clampedVelocity);
                if (vfxPrefab != null)
                {
                    InstantiateVFX(vfxPrefab, other);
                }
            }

            if (enableHapticFeedback)
            {
                TriggerHapticFeedback(gameObject.tag, 0.1f, Mathf.InverseLerp(0, MaxVelocity, clampedVelocity));
            }
        }
    }

    // Method to toggle VFX instantiation
    public void ToggleVFXInstantiation(bool isEnabled)
    {
        instantiateVFX = isEnabled;
    }

    public void ToggleHapticFeedback(bool isEnabled)
    {
        enableHapticFeedback = isEnabled;
    }

    private GameObject SelectVFXPrefabBasedOnVelocity(float velocity)
    {
        
        if (velocity <= 4)
            return whiteSparkVFXPrefab;
        else if (velocity <= 7)
            return yellowSparkVFXPrefab;
        else
            return redSparkVFXPrefab;
        
    }

    private void InstantiateVFX(GameObject vfxPrefab, Collider hitCollider)
    {
        // Assuming the hit is from above and we're looking for the highest point on the collider.
        Vector3 hitPoint = new Vector3(tipTransform.position.x, hitCollider.bounds.max.y, tipTransform.position.z);
        Vector3 estimatedNormal = Vector3.up; // Default normal, works well for flat horizontal surfaces

        // Instantiate the VFX at the hit point, using the estimated normal for rotation.
        Quaternion hitRotation = Quaternion.LookRotation(estimatedNormal);
        GameObject vfxInstance = Instantiate(vfxPrefab, hitPoint, hitRotation);

        ParticleSystem particleSystem = vfxInstance.GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null)
        {
            ApplyParticleModifications(particleSystem, tipVelocity);
        }

        Destroy(vfxInstance, vfxLifetime);
    }

    private void ApplyParticleModifications(ParticleSystem particleSystem, float velocity)
    {
        float scaleMultiplier = CalculateScaleMultiplier(velocity);
        var mainModule = particleSystem.main;

        // Adjust start speed and size based on velocity
        mainModule.startSpeed = Mathf.Lerp(0.01f, 1f, Mathf.Pow(scaleMultiplier, 2));
        mainModule.startSize = scaleMultiplier;

        // Adjust emission rate
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = Mathf.Lerp(10, 100, scaleMultiplier);

        // Adjust shape length for a more natural effect
        var shapeModule = particleSystem.shape;
        shapeModule.length = Mathf.Lerp(0.005f, 0.2f, Mathf.Pow(scaleMultiplier, 2));
    }

    private float CalculateScaleMultiplier(float velocity)
    {
        // Adjust this method as needed to fit the desired effect
        return Mathf.Clamp((velocity / MaxVelocity), 0.5f, 2.5f);
    }

    private void TriggerHapticFeedback(string drumstickTag, float duration, float strength)
    {
        InputDevice device = GetDeviceByDrumstickTag(drumstickTag);

        if (device.isValid)
        {
            device.SendHapticImpulse(0, strength, duration);
        }
    }

    private InputDevice GetDeviceByDrumstickTag(string tag)
    {
        // This method should be implemented to return the correct InputDevice
        // based on whether the tag is LeftDrumstick or RightDrumstick.

        if (tag == "LeftDrumstick")
        {
            return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }
        else if (tag == "RightDrumstick")
        {
            return InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        return new InputDevice(); // (Fallback) ideally never used
    }
}