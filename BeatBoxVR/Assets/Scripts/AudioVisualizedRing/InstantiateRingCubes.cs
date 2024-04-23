using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRingCubes : MonoBehaviour
{
    public GameObject cubePrefab;
    GameObject[] cubes = new GameObject[128];
    public float maxScale;
    public float lerpSpeed;
    public float startScale;

    void Start()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            GameObject instanceCubes = (GameObject)Instantiate(cubePrefab);
            instanceCubes.transform.position = this.transform.position;
            instanceCubes.transform.parent = this.transform;
            instanceCubes.name = "cube" + i;

           
            this.transform.eulerAngles = new Vector3(0, -5.625f * i, 0);
            instanceCubes.transform.position = Vector3.forward * 2f;
            cubes[i] = instanceCubes;
        }
    }


    void Update()
    {
        for(int i =0; i< cubes.Length; i++)
        {
            if (cubes != null)
            {
                cubes[i].transform.localScale = Vector3.Lerp(transform.localScale* startScale,
                    new Vector3(0.05f, AudioSpectrum.audioSamples[i]*100, 0.05f),
                    Time.deltaTime*lerpSpeed);
            }
        }
    }
}
