using UnityEngine;
using UnityEngine.XR;

public class Drumstick : MonoBehaviour
{
    private SoundManager soundManager;

    public GameObject whiteSparkVFXPrefab;
    public GameObject yellowSparkVFXPrefab;
    public GameObject redSparkVFXPrefab;

    public float vfxLifetime = 0.2f;
    private const float MaxVelocity = 10f;


    public Transform tipTransform;
    private Vector3 previousTipPosition;
    private Vector3 tipMovementDirection;
    private float tipVelocity;

    private float lastHitTime = 0f;
    public float hitCooldown = 0.02f;

    public bool instantiateVFX = true; // Flag to control VFX instantiation

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
        // Joshua - TODO Remove cooldown, this is not needed
        // Check if the drumstick is moving downwards and if cooldown has passed
        if (tipMovementDirection.y < 0 && Time.time - lastHitTime > hitCooldown)
        {
            lastHitTime = Time.time; // Update the last hit time
            float clampedVelocity = Mathf.Clamp(tipVelocity, 0, MaxVelocity);
            Debug.Log($"Drumstick hit detected. Velocity: {clampedVelocity}. Collider Tag: {other.tag}");

            // Use the tipTransform's position for sound and VFX instantiation
            Vector3 collisionPoint = tipTransform.position;
            soundManager.PlaySound(other.tag, collisionPoint, clampedVelocity / MaxVelocity);

            if (clampedVelocity > 1 && instantiateVFX) // Check if VFX should be instantiated
            {
                GameObject vfxPrefab = SelectVFXPrefabBasedOnVelocity(clampedVelocity);
                if (vfxPrefab != null)
                {
                    InstantiateVFX(vfxPrefab, collisionPoint, other.transform.position - collisionPoint);
                }
            }

            // Trigger haptic feedback based on the velocity
            float feedbackStrength = Mathf.InverseLerp(0, MaxVelocity, clampedVelocity);
            TriggerHapticFeedback(gameObject.tag, 0.1f, feedbackStrength); // Assuming duration is constant
        }
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
        // Instantiate VFX prefab and adjust its scale based on velocity
        Quaternion hitRotation = Quaternion.LookRotation(direction);
        GameObject vfxInstance = Instantiate(vfxPrefab, position, hitRotation);
        float scaleMultiplier = 1 + (tipVelocity - 1) / (MaxVelocity - 1) * 0.2f;
        vfxInstance.transform.localScale *= scaleMultiplier;
        Destroy(vfxInstance, vfxLifetime);
        Debug.Log($"Instantiated VFX: {vfxPrefab.name} at position: {position}. Scale Multiplier: {scaleMultiplier}");
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