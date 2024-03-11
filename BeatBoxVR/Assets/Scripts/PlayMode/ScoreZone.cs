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
        int hitCount = 0;

        foreach (var noteBlock in noteBlocksInZone)
        {
            if (noteBlock.expectedTag == noteTag && !noteBlock.IsHit)
            {
                Debug.Log($"Hitting note: {noteBlock.name}, Tag: {noteTag}");
                noteBlock.HandleHit();

                if (hitCount >= 4) break;
            }
        }

        Debug.Log($"Total hits processed for {noteTag}: {hitCount}");
    }

    public void HandleInstrumentHit(string instrumentTag)
    {
        foreach (var noteBlock in noteBlocksInZone)
        {
            if (noteBlock.expectedTag == instrumentTag && !noteBlock.IsHit)
            {
                noteBlock.HandleHit(); // Marks the note as hit and updates the game state
                // No need for hitCount logic here unless you have a specific gameplay reason
            }
        }
    }
}
