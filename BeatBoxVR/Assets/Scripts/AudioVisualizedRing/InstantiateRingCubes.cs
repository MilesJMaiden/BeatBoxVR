using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRingCubes : MonoBehaviour
{
    public GameObject cubePrefab;
    GameObject[] cubes = new GameObject[128];
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
            this.transform.eulerAngles = new Vector3(0, -2.8125f * i, 0);
            instanceCubes.transform.position = Vector3.forward * 2;
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
                cubes[i].transform.localScale = new Vector3(1, AudioSpectrum.audioSamples[i]*100, 1);
            }
        }
    }
}
