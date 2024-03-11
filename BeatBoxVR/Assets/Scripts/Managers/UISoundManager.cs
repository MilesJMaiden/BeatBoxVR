using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public AudioClip[] sounds; // Array to hold the sound clips
    private AudioSource audioSource; // AudioSource component

    void Awake()
    {
        // Get the AudioSource component from the GameObject this script is attached to
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("UISoundManager requires an AudioSource component on the same GameObject.");
        }
    }

    // Method to play a random sound from the sounds array
    public void PlayRandomSound()
    {
        if (audioSource != null && sounds.Length > 0)
        {
            // Choose a random AudioClip from the sounds array
            int randomIndex = Random.Range(0, sounds.Length);
            AudioClip randomClip = sounds[randomIndex];

            // Play the selected AudioClip
            audioSource.clip = randomClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No sounds assigned or AudioSource is missing.");
        }
    }
}
