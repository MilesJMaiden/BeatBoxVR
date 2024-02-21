using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform pointer;

    [Header("Select to include in draggable axises")]
    public bool x;
    public bool y;
    public bool z;

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.isValid)
        {
            var currentRaycastPosition = eventData.pointerCurrentRaycast.worldPosition;
            transform.position = currentRaycastPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void Drag()
    {

        float newX = x ? pointer.position.x : transform.position.x;
        float newY = x ? pointer.position.y : transform.position.y;
        float newZ = x ? pointer.position.z : transform.position.z;
        transform.position = new Vector3(newX, newY, pointer.position.z + 3);
    }
}
