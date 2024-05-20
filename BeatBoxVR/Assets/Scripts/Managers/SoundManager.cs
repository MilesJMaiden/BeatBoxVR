using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static EnvironmentManager;
using static SoundManager;

public class SoundManager : MonoBehaviour
{
    //
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
        public string KitName;
        public Sprite KitImage;
        public PercussionSound[] drumKit;
        public Dictionary<string, AudioClip> soundMap = new Dictionary<string, AudioClip>();

        public void BuildSoundDictionary()
        {
            soundMap.Clear();
            foreach (PercussionSound sound in drumKit)
            {
                if (!soundMap.ContainsKey(sound.tag))
                    soundMap.Add(sound.tag, sound.sound);
                else
                    Debug.LogWarning("Duplicate sound tag found: " + sound.tag);
            }
        }
    }

    public List<DrumKit> drumKitList;
    private int currDrumKitIndex = 0;
    public TextMeshProUGUI currDrumKitTMP;
    public Image currDrumKitSprite;


    void Start()
    {
        LoadDrumKit(0); // Load the first drum kit
    }

    public AudioClip GetClipForTag(string tag, bool isHiHatOpen)
    {
        if (tag.Contains("HiHat"))
            tag = isHiHatOpen ? "HiHat Open" : "HiHat Closed";

        if (drumKitList[currDrumKitIndex].soundMap.TryGetValue(tag, out AudioClip clip))
            return clip;

        Debug.LogWarning("Sound clip not found for tag: " + tag);
        return null;
    }

    public void PlaySound(string tag, Vector3 position, float velocity, bool isHiHatOpen = false)
    {
        AudioClip clip = GetClipForTag(tag, isHiHatOpen);
        if (clip != null)
        {
            GameObject soundObject = new GameObject("TemporaryAudio");
            soundObject.transform.position = position;
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.spatialBlend = 1.0f;
            audioSource.volume = Mathf.Clamp(velocity, 0.0f, 1.0f);
            audioSource.pitch = 1.0f + velocity * 0.1f;
            audioSource.Play();
            Destroy(soundObject, clip.length);
        }
        else
        {
            Debug.LogWarning($"No sound found for tag: {tag} with hi-hat state: {(isHiHatOpen ? "open" : "closed")}");
        }
    }

    public void LoadDrumKit(int kitIndex)
    {
        if (kitIndex >= 0 && kitIndex < drumKitList.Count)
        {
            drumKitList[kitIndex].BuildSoundDictionary(); // Build dictionary for the kit
            currDrumKitIndex = kitIndex;
            currDrumKitTMP.text = drumKitList[kitIndex].KitName;
            currDrumKitSprite.sprite = drumKitList[kitIndex].KitImage;
        }
    }

    // Methods for cycling through drum kits
    public void incrementDrumKit()
    {
        int nextIndex = (currDrumKitIndex + 1) % drumKitList.Count;
        LoadDrumKit(nextIndex);
    }

    public void decrementDrumKit()
    {
        int prevIndex = (currDrumKitIndex - 1 + drumKitList.Count) % drumKitList.Count;
        LoadDrumKit(prevIndex);
    }
    public void SetMasterVolume(Slider slider)
    {
        float sliderValue = slider.value;
        float volume = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("MasterVolume", volume);
    }
    public void SetDrumVolume(Slider slider)
    {
        float sliderValue = slider.value;
        float volume = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("DrumVolume", volume);
    }
}
