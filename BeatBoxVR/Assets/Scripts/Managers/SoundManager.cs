using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static EnvironmentManager;
using static SoundManager;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // Assign this in the Inspector

    // Serializable class to hold sound data for drums and cymbals
    [System.Serializable]
    public class PercussionSound
    {
        public string tag;           // Tag to identify the percussion instrument
        public AudioClip sound;      // Sound for the percussion hit
    }

    [System.Serializable]
    public class DrumKit
    {
        public string KitName;           // Tag to identify the percussion instrument
        public Sprite KitImage;
        public PercussionSound[] drumKit;
    }

    // Lists to hold the sounds for drums and cymbals
    public List<PercussionSound> percussionSounds;
    public List<DrumKit> drumKitList;
    private int currDrumKitIndex = 0;
    public TextMeshProUGUI currDrumKitTMP;
    public Image currDrumKitSprite;

    // This method needs to differentiate based on hi-hat state
    public void PlaySound(string tag, Vector3 position, float velocity, bool isHiHatOpen = false)
    {
        // Append tag modification for hi-hat here
        string modifiedTag = tag;
        if (tag == "HiHat")
        {
            modifiedTag = isHiHatOpen ? "HiHat Open" : "HiHat Closed";
        }

        AudioClip clip = GetClipForTag(modifiedTag);
        if (clip)
        {
            AudioSource audioSource = CreateAudioSource(position);
            SetupAudioSource(audioSource, clip, velocity);
        }
        else
        {
            Debug.LogWarning("No sound found for tag: " + modifiedTag);
        }
    }

    private AudioSource CreateAudioSource(Vector3 position)
    {
        GameObject soundObject = new GameObject("TemporaryAudio");
        soundObject.transform.position = position;
        return soundObject.AddComponent<AudioSource>();
    }

    private void SetupAudioSource(AudioSource source, AudioClip clip, float velocity)
    {
        source.clip = clip;
        source.spatialBlend = 1.0f;
        source.volume = Mathf.Clamp(velocity, 0.0f, 1.0f);
        source.pitch = 1.0f + velocity * 0.1f;
        source.Play();
        Destroy(source.gameObject, clip.length);
    }

    private AudioClip GetClipForTag(string tag)
    {
        PercussionSound sound = percussionSounds.Find(ps => ps.tag == tag);
        return sound?.sound;
    }

    // Method to adjust the master volume
    public void SetMasterVolume(float sliderValue)
    {
        // Ensure sliderValue is never 0 to avoid log10(0) which is undefined
        sliderValue = Mathf.Max(sliderValue, 0.0001f);
        float volumeDb = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("MasterVolume", volumeDb);
    }

    // Method to adjust the drum volume
    public void SetDrumVolume(float volume)
    {
        // Convert the volume to a logarithmic scale and set it
        audioMixer.SetFloat("DrumVolume", Mathf.Log10(volume) * 20);
    }

    public void LoadDrumKit(int kitIndex)
    {
        if (kitIndex >= 0 && kitIndex < drumKitList.Count)
        {
            currDrumKitTMP.text = drumKitList[kitIndex].KitName;
            currDrumKitSprite.sprite = drumKitList[kitIndex].KitImage;
            percussionSounds = new List<PercussionSound>(drumKitList[kitIndex].drumKit);
        }
    }

    public void incrementDrumKit()
    {
        currDrumKitIndex = (currDrumKitIndex + 1) % drumKitList.Count;
        LoadDrumKit(currDrumKitIndex);
    }

    public void decrementDrumKit()
    {
        currDrumKitIndex = (currDrumKitIndex - 1 + drumKitList.Count) % drumKitList.Count;
        LoadDrumKit(currDrumKitIndex);
    }

}
