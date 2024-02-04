using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Serializable class to hold sound data for drums and cymbals
    [System.Serializable]
    public class PercussionSound
    {
        public string tag;           // Tag to identify the percussion instrument
        public AudioClip sound;      // Sound for the percussion hit
    }

    // Lists to hold the sounds for drums and cymbals
    public List<PercussionSound> percussionSounds;

    // Method to play a sound based on the tag, position, and velocity
    public void PlaySound(string tag, Vector3 position, float velocity)
    {
        AudioClip clip = GetClipForTag(tag);
        if (clip != null)
        {
            GameObject soundObject = new GameObject("TemporaryAudio");
            soundObject.transform.position = position;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = 1.0f; // Sets the sound to be 3D

            // Adjusts volume and pitch based on the hit velocity
            audioSource.volume = Mathf.Clamp(velocity, 0.0f, 1.0f);
            audioSource.pitch = 1.0f + velocity * 0.1f;

            audioSource.Play();
            Destroy(soundObject, clip.length); // Destroys the audio source after playing
        }
        else
        {
            Debug.LogWarning("No sound found for tag: " + tag);
        }
    }

    // Helper method to get the appropriate AudioClip based on the tag
    private AudioClip GetClipForTag(string tag)
    {
        PercussionSound percussionSound = percussionSounds.Find(ps => ps.tag == tag);
        return percussionSound?.sound;
    }
}
