using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class DetectBlock : MonoBehaviour
{
    public Vector3 size;
    public LayerMask blockLayerMask;
    public Drum drum;

    private List<TutorialHandIndicator> tutorialHandIndicatorInZone = new List<TutorialHandIndicator>();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }

    private void OnTriggerEnter(Collider other)
    {
        TutorialHandIndicator tutorialHandIndicator = other.GetComponent<TutorialHandIndicator>();
        if (tutorialHandIndicator != null)
        {
            tutorialHandIndicatorInZone.Add(tutorialHandIndicator);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TutorialHandIndicator tutorialHandIndicator = other.GetComponent<TutorialHandIndicator>();
        if (tutorialHandIndicator != null)
        {
            tutorialHandIndicatorInZone.Remove(tutorialHandIndicator);
        }
    }

    public void AttemptToHitNoteWithTag(string noteTag)
    {

        foreach (var tutorialHandIndicator in tutorialHandIndicatorInZone)
        {
            if (tutorialHandIndicator.expectedTag == noteTag && !tutorialHandIndicator.IsHit)
            {
                Debug.Log($"Hitting note: {tutorialHandIndicator.name}, Tag: {noteTag}");
                tutorialHandIndicator.HandleHit();

                break;
            }
        }

        Debug.Log($"Total hits processed for {noteTag}");
    }

    public void HandleInstrumentHit(string instrumentTag)
    {
        foreach (var tutorialHandIndicator in tutorialHandIndicatorInZone)
        {
            if (tutorialHandIndicator.expectedTag == instrumentTag && !tutorialHandIndicator.IsHit)
            {
                tutorialHandIndicator.HandleHit(); // Marks the note as hit and updates the game state
                // No need for hitCount logic here unless you have a specific gameplay reason
            }
        }
    }
    public void Update()
    {
        //Missing:
        // HandleMiss();
        // Test



        // change this part : Hit Timing 
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    Collider[] cols = Physics.OverlapBox(transform.position, size / 2, Quaternion.identity, blockLayerMask);
        //    if(cols.Length <= 0 )
        //    {
        //        Debug.Log("Miss");
        //        return;
        //    }

        //    float distance = Vector3.Distance ( cols[0].transform.position,transform.position);
        //    Debug.Log($"distance : {distance}");

        //    if ( distance <= 0.05f )
        //    {
        //        Debug.Log("perfect");
        //    }
        //    else if (distance <= 0.5f)
        //    {
        //        Debug.Log("good");
        //    }
        //}
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
