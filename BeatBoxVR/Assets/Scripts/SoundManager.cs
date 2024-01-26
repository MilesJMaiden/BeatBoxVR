using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public class DrumSound
    {
        public string tag;
        public AudioClip surfaceSound;
        public AudioClip rimSound;
    }

    [System.Serializable]
    public class CymbalSound
    {
        public string tag;
        public AudioClip normalSound;
        public AudioClip accentSound;
    }

    public List<DrumSound> drumSounds;
    public List<CymbalSound> cymbalSounds;

    public void PlaySound(string tag, Vector3 position, float velocity)
    {
        AudioClip clip = GetClipForTag(tag);
        if (clip != null)
        {
            GameObject soundObject = new GameObject("TemporaryAudio");
            soundObject.transform.position = position;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = 1.0f; // Fully 3D sound

            // Volume and pitch scaling based on velocity
            audioSource.volume = Mathf.Clamp(velocity, 0.0f, 1.0f);
            audioSource.pitch = 1.0f + velocity * 0.1f; // Slight pitch variation based on velocity

            audioSource.Play();
            Destroy(soundObject, clip.length);
        }
        else
        {
            Debug.LogWarning("No sound found for tag: " + tag);
        }
    }

    private AudioClip GetClipForTag(string tag)
    {
        DrumSound drumSound = drumSounds.Find(ds => ds.tag == tag);
        if (drumSound != null)
        {
            return tag.EndsWith("Rim") ? drumSound.rimSound : drumSound.surfaceSound;
        }

        CymbalSound cymbalSound = cymbalSounds.Find(cs => cs.tag == tag);
        if (cymbalSound != null)
        {
            return cymbalSound.normalSound; // Replace with appropriate logic for cymbals
        }

        return null;
    }
}
