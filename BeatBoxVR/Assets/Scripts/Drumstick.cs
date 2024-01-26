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

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        float velocity = Mathf.Clamp(rigidBody.velocity.magnitude, 0, MaxVelocity);
        soundManager.PlaySound(other.tag, other.ClosestPointOnBounds(transform.position), velocity / MaxVelocity);

        if (velocity > 1) // Assuming very soft hits don't produce VFX
        {
            // Determines the VFX prefab to use based on the hit collider's tag and the drumstick's velocity.
            // If the drumstick hits a rim (identified by the tag ending in "Rim"), it always uses the whiteSparkVFXPrefab.
            // For other hits (not on the rim), it selects the VFX prefab based on the velocity of the drumstick hit.
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
