using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRingCubes : MonoBehaviour
{
    public GameObject cubePrefab;
    GameObject[] cubes = new GameObject[512];
    public float maxScale;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < cubes.Length; i++)
        {
            GameObject instanceCubes = (GameObject)Instantiate(cubePrefab);
            instanceCubes.transform.position = this.transform.position;
            instanceCubes.transform.parent = this.transform;
            instanceCubes.name = "cube" + i;
            // angle need to be changed with list length
            this.transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            instanceCubes.transform.position = Vector3.forward * 100;
            cubes[i] = instanceCubes;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i< cubes.Length; i++)
        {
            if (cubes != null)
            {
                cubes[i].transform.localScale = new Vector3(5, AudioSpectrum.audioSamples[i], 5);
            }
        }
    }
}
