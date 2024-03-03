using UnityEngine;
using static UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets.DynamicMoveProvider;

public class NoteBlock : MonoBehaviour
{
    public string expectedTag; // The tag this note block expects (e.g., "HiHat", "SnareDrum")
    public GameObject hitVFXPrefab; // VFX to instantiate on hit
    public GameObject missVFXPrefab; // VFX to instantiate on miss
    public float destroyDelay = 0.2f; // Delay before destruction

    private Vector3 movementDirection;

    public float moveSpeed;

    public void InitializeNoteBlock(Vector3 direction, float speed)
    {
        movementDirection = direction;
        moveSpeed = speed;
    }

    private void Update()
    {
        transform.position += movementDirection * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ScoreZone"))
        {
            Drumstick drumstick = other.GetComponentInParent<Drumstick>();
            if (drumstick != null && drumstick.gameObject.tag == expectedTag)
            {
                HandleHit();
            }
        }
        else if (other.CompareTag("MissZone"))
        {
            HandleMiss();
        }
    }

    public void HandleHit()
    {
        if (hitVFXPrefab != null)
        {
            GameObject hitVFXInstance = Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
            Destroy(hitVFXInstance, destroyDelay); // Destroy the hit VFX after the specified delay
        }

        // Correct instrument was hit
        PlayModeManager.Instance.IncrementStreak(); // Increment streak
        PlayModeManager.Instance.UpdateScore(1); // Update score considering streak multiplier

        Destroy(gameObject, destroyDelay); // Destroy the note block itself after the delay
    }

    private void HandleMiss()
    {
        Debug.Log("Miss detected for: " + gameObject.name); // Debugging line
        if (missVFXPrefab != null)
        {
            GameObject missVFXInstance = Instantiate(missVFXPrefab, transform.position, Quaternion.identity);
            Destroy(missVFXInstance, destroyDelay); // Destroy the miss VFX after the specified delay
        }

        PlayModeManager.Instance.ResetStreak(); // Reset only the streak on miss

        Destroy(gameObject, destroyDelay); // Destroy the note block itself after the delay
    }
}
