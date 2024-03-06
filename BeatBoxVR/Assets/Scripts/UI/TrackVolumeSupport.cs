using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;

public class TrackVolumeSupport : MonoBehaviour
{

    public AudioSource trackClip;
    public TextMeshProUGUI trackVolumeDisplay;
    private float volumeConversion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        convertAudioVolume(trackClip.volume);
    }

    public void convertAudioVolume(float volume)
    {
        volumeConversion = volume * 100;
        trackVolumeDisplay.text = volumeConversion.ToString("0") + "%";
    }
}
