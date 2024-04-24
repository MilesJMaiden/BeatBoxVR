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
        

        
        if (eventPos.y >= 1.23f && eventPos.y <= 1.27f)
            if (eventPos.x >= .99f && eventPos.x <= 1.95f)
            {
 
                newTime = 0.03f + (((eventData.pointerPressRaycast.worldPosition.x - 0.99f) * (m_PlayAlongDetailLoader.SongPlaylist[m_PlayAlongButtonManager.currentSongID].songLengthSec)) 
                    / (1.95f - 0.99f));

                Debug.Log("New Time: " + newTime);

                m_PlayAlongDetailLoader.currBalancedTrack.time = newTime;
                m_PlayAlongDetailLoader.currDrumTrack.time = newTime;
                m_PlayAlongButtonManager.setCurrentAudioTime(newTime);
            }

    }
}

