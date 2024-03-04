using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DetectBlock : MonoBehaviour
{
    public Vector3 size;
    public LayerMask blockLayerMask;
    public Drum drum;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }

    public void Update()
    {
        
        // change this part : Hit Timing 
        if (Input.GetKeyDown(KeyCode.T))
        {
            Collider[] cols = Physics.OverlapBox(transform.position, size / 2, Quaternion.identity, blockLayerMask);
            if(cols.Length <= 0 )
            {
                Debug.Log("Miss");
                return;
            }

            float distance = Vector3.Distance ( cols[0].transform.position,transform.position);
            Debug.Log($"distance : {distance}");

            if ( distance <= 0.05f )
            {
                Debug.Log("perfect");
            }
            else if (distance <= 0.5f)
            {
                Debug.Log("good");
            }
        }
    }
}

public enum Drum
{
    HiHat,
    Snare,
    Crash,
    Kick,
    Small,
    Medium,
    Floor,
    Ride,
    Splash
}
