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
        soundManager.PlaySound(other.tag, other.ClosestPointOnBounds(transform.position), velocity / MaxVelocity); // Volume scales from 0 to 1

        if (velocity > 1) // Assuming velocity < 1 is too soft for VFX
        {
            GameObject vfxPrefab = SelectVFXPrefabBasedOnVelocity(velocity);
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
