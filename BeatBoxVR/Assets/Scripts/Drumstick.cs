using UnityEngine;

public class Drumstick : MonoBehaviour
{
    private SoundManager soundManager;

    public GameObject whiteSparkVFXPrefab;
    public GameObject yellowSparkVFXPrefab;
    public GameObject redSparkVFXPrefab;

    public float vfxLifetime = 0.2f;
    private const float MaxVelocity = 10f; // Maximum considered velocity


    public Transform tipTransform; // Assign this in the Inspector
    private Vector3 previousTipPosition;
    private Vector3 tipMovementDirection;
    private float tipVelocity;

    private float lastHitTime = 0f;
    public float hitCooldown = 0.02f; // Adjust as needed

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
        // Check if the drumstick is moving downwards and if cooldown has passed
        if (tipMovementDirection.y < 0 && Time.time - lastHitTime > hitCooldown)
        {
            lastHitTime = Time.time; // Update the last hit time
            float clampedVelocity = Mathf.Clamp(tipVelocity, 0, MaxVelocity);
            Debug.Log($"Drumstick hit detected. Velocity: {clampedVelocity}. Collider Tag: {other.tag}");

            // Use the tipTransform's position for sound and VFX instantiation
            Vector3 collisionPoint = tipTransform.position;

            soundManager.PlaySound(other.tag, collisionPoint, clampedVelocity / MaxVelocity);

            if (clampedVelocity > 1) // Assuming very soft hits don't produce VFX
            {
                GameObject vfxPrefab = other.tag.EndsWith("Rim") ? whiteSparkVFXPrefab : SelectVFXPrefabBasedOnVelocity(clampedVelocity);
                if (vfxPrefab != null)
                {
                    Quaternion hitRotation = Quaternion.LookRotation(other.transform.position - collisionPoint);
                    GameObject vfxInstance = Instantiate(vfxPrefab, collisionPoint, hitRotation);

                    float scaleMultiplier = 1 + (clampedVelocity - 1) / (MaxVelocity - 1) * 0.2f; // Scale from 1 to 1.2
                    vfxInstance.transform.localScale *= scaleMultiplier;

                    Destroy(vfxInstance, vfxLifetime);
                    Debug.Log($"Instantiated VFX: {vfxPrefab.name} at position: {collisionPoint}. Scale Multiplier: {scaleMultiplier}");
                }
            }
        }
    }


    private GameObject SelectVFXPrefabBasedOnVelocity(float velocity)
    {
        if (velocity <= 2)
            return whiteSparkVFXPrefab;
        else if (velocity <= 4)
            return yellowSparkVFXPrefab;
        else
            return redSparkVFXPrefab;
    }
}