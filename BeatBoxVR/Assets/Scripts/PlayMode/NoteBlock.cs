using UnityEngine;

public class NoteBlock : MonoBehaviour
{
    public string expectedTag; // The tag this note block expects (e.g., "HiHat", "SnareDrum")
    public GameObject hitVFXPrefab; // VFX to instantiate on hit
    public GameObject missVFXPrefab; // VFX to instantiate on miss
    public float destroyDelay = 0.2f; // Delay before destruction

    public float moveSpeed;

    public void InitializeNoteBlock(float speed)
    {
        moveSpeed = speed;
    }

    private void Update()
    {
        // Move the note block forward along its local Z axis at songSpeed
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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

    // Adjusted to public so it can be called from the PercussionInstrument class
    public void HandleHit()
    {
        // Instantiate hit VFX
        if (hitVFXPrefab != null)
        {
            Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
        }

        // Correct instrument was hit
        PlayModeManager.Instance.IncrementStreak(); // Increment streak
        PlayModeManager.Instance.UpdateScore(1); // Update score considering streak multiplier

        Destroy(gameObject, destroyDelay);
    }

    private void HandleMiss()
    {
        // Instantiate miss VFX
        if (missVFXPrefab != null)
        {
            Instantiate(missVFXPrefab, transform.position, Quaternion.identity);
        }

        PlayModeManager.Instance.ResetStreak(); // Reset only the streak on miss

        Destroy(gameObject, destroyDelay);
    }
}
