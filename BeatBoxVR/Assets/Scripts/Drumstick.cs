using UnityEngine;

public class Drumstick : MonoBehaviour
{
    private Rigidbody rigidBody;
    private SoundManager soundManager;

    public GameObject whiteSparkVFXPrefab;
    public GameObject yellowSparkVFXPrefab;
    public GameObject redSparkVFXPrefab;
    public float vfxLifetime = 0.2f;
    private const float MaxVelocity = 10f; // Maximum considered velocity

    public Collider drumstickTipCollider; // Collider for the tip of the drumstick
    public Collider drumstickBodyCollider; // Collider for the body (+ shoulder) of the drumstick

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene");
        }

        if (drumstickTipCollider == null || drumstickBodyCollider == null)
        {
            Debug.LogError("Drumstick colliders not assigned in " + gameObject.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        float velocity = Mathf.Clamp(rigidBody.velocity.magnitude, 0, MaxVelocity);
        Debug.Log($"Drumstick hit detected. Velocity: {velocity}. Collider Tag: {other.tag}");


        soundManager.PlaySound(other.tag, other.ClosestPointOnBounds(transform.position), velocity / MaxVelocity);

        if (velocity > 1) // Assuming very soft hits don't produce VFX
        {
            // This line selects the appropriate VFX prefab based on the part of the drumstick that made contact and the velocity of the hit.
            // If the drumstick hits a 'Rim' (determined by checking if the tag of the collided object ends with "Rim"), 
            // it always uses the whiteSparkVFXPrefab, which is designated for rim hits.
            // If the hit is not on a rim, it calls the SelectVFXPrefabBasedOnVelocity method to choose the VFX prefab 
            // based on the velocity of the drumstick hit. This allows for different VFX (white, yellow, red) 
            // at different velocities, providing visual feedback corresponding to the intensity of the hit.
            GameObject vfxPrefab = other.tag.EndsWith("Rim") ? whiteSparkVFXPrefab : SelectVFXPrefabBasedOnVelocity(velocity);
            if (vfxPrefab != null)
            {
                Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
                Quaternion hitRotation = Quaternion.LookRotation(other.transform.position - hitPoint);
                GameObject vfxInstance = Instantiate(vfxPrefab, hitPoint, hitRotation);

                // Scale VFX size based on velocity
                float scaleMultiplier = 1 + (velocity - 1) / (MaxVelocity - 1) * 0.2f; // Scale from 1 to 1.2
                vfxInstance.transform.localScale *= scaleMultiplier;

                Destroy(vfxInstance, vfxLifetime);
                Debug.Log($"Instantiated VFX: {vfxPrefab.name} at position: {hitPoint}. Scale Multiplier: {scaleMultiplier}");

            }
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
}