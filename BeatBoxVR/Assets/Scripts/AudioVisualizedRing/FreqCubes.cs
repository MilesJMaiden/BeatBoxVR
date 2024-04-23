using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreqCubes : MonoBehaviour
{
    public int bands;
    public float startScale, scaleMultiplier;
    public  float lerpSpeed;
    public bool useBuffer = true;
    
    void Update()
    {
        if (useBuffer)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
             new Vector3(transform.localScale.x, (AudioSpectrum.bandBuffer[bands] * scaleMultiplier) + startScale, transform.localScale.z),
             Time.deltaTime * lerpSpeed);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale,
             new Vector3(transform.localScale.x, (AudioSpectrum.frequencyBand[bands] * scaleMultiplier) + startScale, transform.localScale.z),
             Time.deltaTime * lerpSpeed);
        }
        
    }
}
