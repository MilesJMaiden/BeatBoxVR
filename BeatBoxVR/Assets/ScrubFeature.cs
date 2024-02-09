using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ScrubFeature : MonoBehaviour, IPointerDownHandler
{

    public PlayAlongButtonManager m_PlayAlongButtonManager;
    public PlayAlongDetailLoader m_PlayAlongDetailLoader;
    private float newTime;
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 eventPos = eventData.position;
        Debug.Log(eventPos);

        
        if (eventPos.y >= 180 && eventPos.y <= 350)
            if (eventPos.x >= 670 && eventPos.x <= 3160)
            {
                newTime = 0 + (((eventData.position.x - 670) * (m_PlayAlongDetailLoader.SongPlaylist[m_PlayAlongButtonManager.currentSongID].songLengthSec)) 
                    / (3160 - 670));

                m_PlayAlongDetailLoader.currBalancedTrack.time = newTime;
                m_PlayAlongDetailLoader.currDrumTrack.time = newTime;
                m_PlayAlongButtonManager.setCurrentAudioTime(newTime);
            }

    }
}

