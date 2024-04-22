using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    //public AudioSource audioSource;
    // Length of array need to be changed
    [SerializeField]public static float[] audioSamples = new float[128];

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        GetSpectrumAudioSource();
        
        
    }
    void GetSpectrumAudioSource()
    {
        AudioListener.GetSpectrumData(audioSamples, 0, FFTWindow.Blackman);
    }
}
