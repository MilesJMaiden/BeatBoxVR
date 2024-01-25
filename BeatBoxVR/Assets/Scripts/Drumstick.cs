using UnityEngine;

public class Drumstick : MonoBehaviour
{
    private Rigidbody rigidBody;
    private SoundManager soundManager;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Find the SoundManager in the scene
        soundManager = FindObjectOfType<SoundManager>();
        if (soundManager == null)
        {
            Debug.LogError("SoundManager not found in the scene");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        float velocity = rigidBody.velocity.magnitude;
        soundManager.PlaySound(other.tag, other.ClosestPointOnBounds(transform.position), velocity);
    }
}
