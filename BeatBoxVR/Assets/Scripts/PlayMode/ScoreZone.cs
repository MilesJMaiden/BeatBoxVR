using System.Collections.Generic;
using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private List<NoteBlock> noteBlocksInZone = new List<NoteBlock>();

    private void OnTriggerEnter(Collider other)
    {
        NoteBlock noteBlock = other.GetComponent<NoteBlock>();
        if (noteBlock != null)
        {
            noteBlocksInZone.Add(noteBlock);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        NoteBlock noteBlock = other.GetComponent<NoteBlock>();
        if (noteBlock != null)
        {
            noteBlocksInZone.Remove(noteBlock);
        }
    }

    // Modified to allow handling multiple note hits
    public void AttemptToHitNoteWithTag(string noteTag)
    {
        // Track the number of hits to ensure no more than 4 notes are processed
        int hitCount = 0;

        foreach (var noteBlock in noteBlocksInZone)
        {
            if (noteBlock.expectedTag == noteTag && !noteBlock.IsHit)
            {
                noteBlock.HandleHit();
                hitCount++;

                // Break out of the loop if the maximum number of notes has been hit
                if (hitCount >= 4)
                {
                    break;
                }
            }
        }
    }
}
