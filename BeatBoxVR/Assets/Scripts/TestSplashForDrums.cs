using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSplashForDrums : MonoBehaviour
{
    [SerializeField]private GameObject slpashForDrumsPrefab;
    [SerializeField] private Transform splashTransfrom;
    [SerializeField] private float vfxLifetime;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InstantiateVFX(slpashForDrumsPrefab, splashTransfrom.position);
        }
        
    }
    private void InstantiateVFX(GameObject vfxPrefab, Vector3 position)
    {
        // Instantiate the VFX prefab and destroy it after vfxLifetime
        var vfxInstance = Instantiate(vfxPrefab, position, Quaternion.identity);
        Destroy(vfxInstance, vfxLifetime);
    }
}
