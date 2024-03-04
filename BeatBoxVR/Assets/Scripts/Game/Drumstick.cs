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

            Vector3 collisionPoint = tipTransform.position;
            soundManager.PlaySound(other.tag, collisionPoint, clampedVelocity / MaxVelocity);

            // Assume the collider's center is a good approximation for VFX spawn location
            Vector3 spawnPosition = other.bounds.center + new Vector3(0, 0.1f, 0); // Offset above the collider
            
            if (clampedVelocity > 1 && instantiateVFX)
            {
                GameObject vfxPrefab = SelectVFXPrefabBasedOnVelocity(clampedVelocity);
                if (vfxPrefab != null)
                {
                    InstantiateVFX(vfxPrefab, spawnPosition, Vector3.up); // Use upward direction for consistency
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

    private void InstantiateVFX(GameObject vfxPrefab, Vector3 position, Vector3 direction)
    {
        
        Quaternion hitRotation = Quaternion.LookRotation(direction);
        GameObject vfxInstance = Instantiate(vfxPrefab, position, hitRotation);

        float scaleMultiplier = CalculateScaleMultiplier(tipVelocity);
        ParticleSystem particleSystem = vfxInstance.GetComponentInChildren<ParticleSystem>();

        if (particleSystem != null)
        {
            // Adjust start speed based on scaleMultiplier
            var mainModule = particleSystem.main;
            // Use a more dramatic range for startSpeed for slow hits
            mainModule.startSpeed = Mathf.Lerp(0.01f, 1f, Mathf.Pow(scaleMultiplier, 2)); // Use square to exaggerate difference at lower end

            // Adjust emission rate over time
            var emissionModule = particleSystem.emission;
            // Decrease minimum rate for slower hits
            emissionModule.rateOverTime = Mathf.Lerp(0.5f, 75f, Mathf.Pow(scaleMultiplier, 2)); // Adjusted range and used square

            // Adjust shape length
            var shapeModule = particleSystem.shape;
            // Use a narrower range for the length to make it even smaller for slow hits
            shapeModule.length = Mathf.Lerp(0.005f, 0.2f, Mathf.Pow(scaleMultiplier, 2)); // Decreased minimum length and used square
        }

        Destroy(vfxInstance, vfxLifetime);
        Debug.Log($"Instantiated VFX: {vfxPrefab.name} at position: {position}. Scale Multiplier: {scaleMultiplier}");
        
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