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
        Vector2 eventPos = eventData.pointerPressRaycast.worldPosition;
        Debug.Log(eventPos);
        

        
        if (eventPos.y >= 1.28f && eventPos.y <= 1.4f)
            if (eventPos.x >= 0.78 && eventPos.x <= 2.12f)
            {
                newTime = 0 + (((eventData.pointerPressRaycast.worldPosition.x - 0.78f) * (m_PlayAlongDetailLoader.SongPlaylist[m_PlayAlongButtonManager.currentSongID].songLengthSec)) 
                    / (2.12f - 0.78f));

                m_PlayAlongDetailLoader.currBalancedTrack.time = newTime;
                m_PlayAlongDetailLoader.currDrumTrack.time = newTime;
                m_PlayAlongButtonManager.setCurrentAudioTime(newTime);
            }

    }
}

