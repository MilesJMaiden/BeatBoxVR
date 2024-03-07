using UnityEngine;
using UnityEngine.XR;

public class Drumstick : MonoBehaviour
{
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
        if (tipMovementDirection.y < 0)
        {
            float clampedVelocity = GetCurrentVelocity();
            Debug.Log($"Drumstick hit detected. Velocity: {clampedVelocity}. Collider Tag: {other.tag}");

            soundManager.PlaySound(other.tag, tipTransform.position, clampedVelocity / MaxVelocity);

            if (clampedVelocity > 1 && instantiateVFX)
            {
                GameObject vfxPrefab = SelectVFXPrefabBasedOnVelocity(clampedVelocity);
                if (vfxPrefab != null)
                {
                    InstantiateVFX(vfxPrefab, tipTransform.position, other);
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

    private void InstantiateVFX(GameObject vfxPrefab, Vector3 position, Collider collider)
    {
        // Calculate the highest point on the collider's surface along the y-axis.
        float highestPoint = collider.bounds.max.y;
        Vector3 spawnPosition = new Vector3(position.x, highestPoint, position.z);

        // Instantiate VFX at the calculated spawn position
        Quaternion hitRotation = Quaternion.LookRotation(tipMovementDirection.normalized);
        GameObject vfxInstance = Instantiate(vfxPrefab, spawnPosition, hitRotation);

        // Apply scaling based on velocity
        ApplyParticleScaling(vfxInstance, tipVelocity);

        // Destroy the VFX after its lifetime expires
        Destroy(vfxInstance, vfxLifetime);
    }

    private void ApplyParticleScaling(GameObject vfxInstance, float velocity)
    {
        float scaleMultiplier = CalculateScaleMultiplier(velocity);
        ParticleSystem particleSystem = vfxInstance.GetComponentInChildren<ParticleSystem>();
        if (particleSystem != null)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startSpeedMultiplier = Mathf.Lerp(0.1f, 1f, scaleMultiplier);
            mainModule.startSizeMultiplier = scaleMultiplier;

            var emissionModule = particleSystem.emission;
            emissionModule.rateOverTimeMultiplier = Mathf.Lerp(10f, 100f, scaleMultiplier);
        }
    }

    // Method to calculate scale multiplier based on tip velocity, now with more variance
    private float CalculateScaleMultiplier(float velocity)
    {
        // Example adjustment: use exponential or quadratic scaling for more noticeable differences
        return Mathf.Lerp(0.5f, 3f, Mathf.Pow(velocity / MaxVelocity, 2));
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