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

    public void AttemptToHitNoteWithTag(string noteTag)
    {
        foreach (var noteBlock in noteBlocksInZone)
        {
            if (noteBlock.expectedTag == noteTag)
            {
                noteBlock.HandleHit(); // Trigger the hit logic on the NoteBlock
                return; // Exit the loop once a matching note is hit
            }
        }
    }
}
