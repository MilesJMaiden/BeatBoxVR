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
        public AudioClip accentSound; // Different sound for edge or bell, if applicable
    }

    public List<DrumSound> drumSounds;
    public List<CymbalSound> cymbalSounds;
    public GameObject audioSourcePrefab;

    public void PlaySound(string tag, float velocity)
    {
        //fFind a matching drum sound
        DrumSound drumSound = drumSounds.Find(ds => ds.tag == tag);
        if (drumSound != null)
        {
            AudioClip clip = GetDrumClip(drumSound, velocity);
            PlayClip(clip);
            return;
        }

        // Find Matching Cymbale Sound
        CymbalSound cymbalSound = cymbalSounds.Find(cs => cs.tag == tag);
        if (cymbalSound != null)
        {
            AudioClip clip = GetCymbalClip(cymbalSound, velocity);
            PlayClip(clip);
            return;
        }

        Debug.LogWarning("No sound found for tag: " + tag);
        PlayFallbackSound(velocity);
    }

    private AudioClip GetDrumClip(DrumSound drumSound, float velocity)
    {
        // Choose between surfaceSound and rimSound
        bool isRim = drumSound.tag.EndsWith("Rim");
        return isRim ? drumSound.rimSound : drumSound.surfaceSound;
    }

    private AudioClip GetCymbalClip(CymbalSound cymbalSound, float velocity)
    {
        // Implement logic to choose between normalSound and accentSound
        return cymbalSound.normalSound; // Replace with logic !TODO!
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            GameObject soundObject = Instantiate(audioSourcePrefab, transform);
            AudioSource audioSource = soundObject.GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(soundObject, clip.length);
        }
    }

    private void PlayFallbackSound(float velocity)
    {
        // Optionally play a default sound or handle the case when no sound is found
        // Example: Play default sound or log a message
    }
}
