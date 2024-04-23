using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    
    public static float[] audioSamples = new float[128];
    public static float[] frequencyBand = new float[7];
    public static float[] bandBuffer = new float[7];
    float[] bufferDecrease = new float[7];


    void Update()
    {
        // Get audio from the environment
        GetSpectrumAudioSource();

        //  Start Frequency band
        MakeFrequencyBands();

        // Band scaling buffering
        BandBuffer();
        
    }
    void GetSpectrumAudioSource()
    {
        AudioListener.GetSpectrumData(audioSamples, 0, FFTWindow.Blackman);
    }
    void MakeFrequencyBands()
    {
        // Each band takes the ((power of 2)*songaveragefrequency/spectrumsampleCount) frequency in average
        int Count = 0;
        for (int i = 0; i <frequencyBand.Length; i++)
        {
            float average = 0;
            int SampleCount = (int)Mathf.Pow(2, i) * 2;
            for(int j = 0; j < frequencyBand.Length; j++)
            {
                average += audioSamples[Count] * (Count + 1);
                Count++;
            }
            average /= Count;
            frequencyBand[i] = average;
        }
    }

    void BandBuffer()
    {
        for (int i = 0;i < frequencyBand.Length;i++)
        {
            if (frequencyBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = frequencyBand[i];
                bufferDecrease[i] = 0.005f;
            }

            if (frequencyBand[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }
}
