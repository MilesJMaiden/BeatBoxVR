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

    // This method gets called when something enters its trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the score zone
        if (other.CompareTag("ScoreZone"))
        {
            // Assuming the parent of the collider has a Drumstick component
            Drumstick drumstick = other.GetComponentInParent<Drumstick>();
            if (drumstick != null && drumstick.tag == expectedTag)
            {
                // Correct instrument was hit
                HandleHit();
            }
            else
            {
                // Incorrect instrument was hit or missed
                HandleMiss();
            }
        }
    }

    private void HandleHit()
    {
        // Instantiate hit VFX
        if (hitVFXPrefab != null)
        {
            Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
        }

        // Update the score through the PlayModeManager
        PlayModeManager.Instance?.UpdateScore(1);

        Destroy(gameObject, destroyDelay);
    }

    private void HandleMiss()
    {
        // Instantiate miss VFX
        if (missVFXPrefab != null)
        {
            Instantiate(missVFXPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, destroyDelay);
    }
}
