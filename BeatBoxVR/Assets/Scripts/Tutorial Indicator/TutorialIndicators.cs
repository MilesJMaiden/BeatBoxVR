using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialIndicators : MonoBehaviour
{
    public GameObject prefab;

    public string notation;
    //public float speed;

    public float nextPlayTime;
    public int currentNote;

    void Start()
    {
        
    }


    void Update()
    {
        if (notation.Length == 0)
        {
            return;
        }

        if(Time.time > nextPlayTime)
        {
            nextPlayTime = Time.time + TutorialSpeedController.Instance.speed;
            if(notation.Substring(currentNote,1) != "0")
            {
                CreateNote(notation.Substring(currentNote,1));
            }
            currentNote++;
            if(currentNote >= notation.Length)
            {
                currentNote = 0;
            }
        }
    }

    void CreateNote(string hand)
    {
        GameObject tempGO = Instantiate(prefab, transform);
        tempGO.transform.localPosition = new Vector3(0, 0, 5);
        if(hand == "L")
        {
            tempGO.GetComponent<TutorialHandIndicator>().leftHand.SetActive(true);
        }
        if (hand == "R")
        {
            tempGO.GetComponent<TutorialHandIndicator>().rightHand.SetActive(true);
        }
        iTween.MoveTo(tempGO, iTween.Hash("position", new Vector3(0, 0, 0), "time", 2.2f, "easeType", iTween.EaseType.linear, "islocal", true));
        Destroy(tempGO, 2.2f);

    }

    public void StopNote()
    {
       
    }
}
