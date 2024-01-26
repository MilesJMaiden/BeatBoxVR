using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Serializable class to hold sound data for drums
    [System.Serializable]
    public class DrumSound
    {
        public string tag;           // Tag to identify the drum
        public AudioClip surfaceSound;  // Sound for hitting the drum surface
        public AudioClip rimSound;      // Sound for hitting the drum rim
    }

    // Serializable class to hold sound data for cymbals
    [System.Serializable]
    public class CymbalSound
    {
        public string tag;           // Tag to identify the cymbal
        public AudioClip normalSound;    // Normal sound for cymbal hit
        public AudioClip accentSound;    // Accent sound for cymbal hit (not used in this version)
    }

    // Lists to hold the drum and cymbal sounds
    public List<DrumSound> drumSounds;
    public List<CymbalSound> cymbalSounds;

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
        DrumSound drumSound = drumSounds.Find(ds => ds.tag == tag);
        if (drumSound != null)
        {
            // Chooses rim or surface sound for drums based on the tag
            return tag.EndsWith("Rim") ? drumSound.rimSound : drumSound.surfaceSound;
        }

        CymbalSound cymbalSound = cymbalSounds.Find(cs => cs.tag == tag);
        if (cymbalSound != null)
        {
            // Currently returns only the normal sound for cymbals
            return cymbalSound.normalSound;
        }

        return null;
    }
}
