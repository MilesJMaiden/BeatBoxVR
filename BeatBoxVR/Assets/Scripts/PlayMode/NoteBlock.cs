using UnityEngine;

public class NoteBlock : MonoBehaviour
{
    public float songSpeed; // Speed at which the note moves, assigned from the PlayModeManager
    public GameObject missVFXPrefab; // VFX to instantiate on miss
    public GameObject hitVFXPrefab; // VFX to instantiate on hit
    public float destroyDelay = 0.2f; // Time before the note is destroyed after missing or hitting

    private void Update()
    {
        // Move the note block forward along its local Z axis
        transform.Translate(Vector3.forward * songSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MissZone"))
        {
            // Instantiate miss VFX if assigned
            if (missVFXPrefab != null)
            {
                Instantiate(missVFXPrefab, transform.position, Quaternion.identity);
            }

            // Destroy the note block after a delay
            Destroy(gameObject, destroyDelay);
        }
        else if (other.CompareTag("HitZone"))
        {
            // Logic for when a note block is hit successfully
            // Instantiate hit VFX if assigned
            if (hitVFXPrefab != null)
            {
                Instantiate(hitVFXPrefab, transform.position, Quaternion.identity);
            }

            // Destroy the note block immediately or after a delay
            Destroy(gameObject, destroyDelay);
        }
    }
}
