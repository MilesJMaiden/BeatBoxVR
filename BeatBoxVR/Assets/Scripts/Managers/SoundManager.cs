using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    //
    [Header("Audio Mixer")]
    public AudioMixer audioMixer; // Assign this in the Inspector

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

    public List<PercussionSound> percussionSounds;
    public List<DrumKit> drumKitList;
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private int currDrumKitIndex = 0;
    public TextMeshProUGUI currDrumKitTMP;
    public Image currDrumKitSprite;

    void Start()
    {
        LoadAllSounds();
    }

    private void LoadAllSounds()
    {
        foreach (var sound in percussionSounds)
        {
            if (!audioClips.ContainsKey(sound.tag))
                audioClips.Add(sound.tag, sound.sound);
        }
    }

    public void PlaySound(string tag, Vector3 position, float velocity, bool isHiHatOpen = false)
    {
        string adjustedTag = AdjustTagBasedOnHiHatState(tag, isHiHatOpen);
        if (audioClips.TryGetValue(adjustedTag, out AudioClip clip))
        {
            AudioSource source = AudioSourcePool.Instance.GetSource();
            source.transform.position = position;
            source.clip = clip;
            source.volume = Mathf.Clamp(velocity, 0.0f, 1.0f);
            source.pitch = 1.0f + velocity * 0.1f;
            source.Play();

            StartCoroutine(ReturnSourceToPool(source, clip.length));
        }
        else
        {
            Debug.LogWarning($"No sound found for tag: {adjustedTag}");
        }
    }

    private string AdjustTagBasedOnHiHatState(string tag, bool isHiHatOpen)
    {
        return tag.Contains("HiHat") ? (isHiHatOpen ? "HiHat Open" : "HiHat Closed") : tag;
    }

    private IEnumerator ReturnSourceToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioSourcePool.Instance.ReturnSource(source);
    }

    public void SetMasterVolume(float sliderValue)
    {
        sliderValue = Mathf.Max(sliderValue, 0.0001f);  // Ensure slider value is never zero
        float volumeDb = Mathf.Log10(sliderValue) * 20;
        audioMixer.SetFloat("MasterVolume", volumeDb);
    }

    public void SetDrumVolume(float volume)
    {
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
