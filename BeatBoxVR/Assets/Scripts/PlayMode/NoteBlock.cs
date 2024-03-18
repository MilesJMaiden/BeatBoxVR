using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider;

public class NoteBlock : MonoBehaviour
{
    public string expectedTag; // The tag this note block expects (e.g., "HiHat", "SnareDrum")
    public GameObject hitVFXPrefab; // VFX to instantiate on hit
    public GameObject missVFXPrefab; // VFX to instantiate on miss
    public float destroyDelay = 0.5f; // Delay before destruction

    private Vector3 movementDirection;
    public float moveSpeed;

    public bool IsHit { get; private set; } = false;

    public void InitializeNoteBlock(Vector3 direction, float speed)
    {
        movementDirection = direction;
        moveSpeed = speed;
    }

    private void Update()
    {
        if (!GameManager.isGamePaused)
        {
            transform.position += movementDirection * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MissZone"))
        {
            HandleMiss();
        }
    }

    public void HandleHit()
    {
        if (!IsHit) // Check if the note has not already been hit
        {
            if (hitVFXPrefab != null)
            {
                GameObject hitVFXInstance = Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
                Destroy(hitVFXInstance, destroyDelay); // Destroy the hit VFX after the specified delay
            }

            // Correct instrument was hit
            PlayModeManager.Instance.UpdateScore(1); // Update score accoring to streak multiplier

            Destroy(gameObject); // Destroy the note block itself after the delay
            IsHit = true; // Mark the note as hit
        }
    }

    private void HandleMiss()
    {
        if (!IsHit) // Only process the miss if the note hasn't been hit
        {
            Debug.Log("Miss detected for: " + gameObject.name); // Debug
            if (missVFXPrefab != null)
            {
                GameObject missVFXInstance = Instantiate(missVFXPrefab, transform.position, Quaternion.identity);
                Destroy(missVFXInstance, destroyDelay); // Destroy the miss VFX after the specified delay
            }

            PlayModeManager.Instance.ResetStreak(); // Reset
            Destroy(gameObject);
        }
    }
}
