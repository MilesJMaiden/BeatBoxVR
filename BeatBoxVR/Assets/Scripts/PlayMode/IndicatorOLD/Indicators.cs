using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicators : MonoBehaviour
{
    public GameObject prefab;

    public string notation;
    //public float speed;

    private float nextPlayTime;
    private int currentNote;
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
            nextPlayTime = Time.time + SpeedController.Instance.speed;
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
            tempGO.GetComponent<HandIndicator>().leftHand.SetActive(true);
        }
        if (hand == "R")
        {
            tempGO.GetComponent<HandIndicator>().rightHand.SetActive(true);
        }
        iTween.MoveTo(tempGO, iTween.Hash("position", new Vector3(0, 0, 0), "time", 2f, "easeType", iTween.EaseType.linear, "islocal", true));
        Destroy(tempGO, 2f);
    }
}
