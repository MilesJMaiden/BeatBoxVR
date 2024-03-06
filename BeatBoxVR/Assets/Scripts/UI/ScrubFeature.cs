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
        

        
        if (eventPos.y >= .72f && eventPos.y <= .84f)
            if (eventPos.x >= 0.78 && eventPos.x <= 2.14f)
            {
                newTime = 0.067f + (((eventData.pointerPressRaycast.worldPosition.x - 0.78f) * (m_PlayAlongDetailLoader.SongPlaylist[m_PlayAlongButtonManager.currentSongID].songLengthSec)) 
                    / (2.14f - 0.78f));

                Debug.Log("New Time: " + newTime);

                m_PlayAlongDetailLoader.currBalancedTrack.time = newTime;
                m_PlayAlongDetailLoader.currDrumTrack.time = newTime;
                m_PlayAlongButtonManager.setCurrentAudioTime(newTime);
            }

    }
}

