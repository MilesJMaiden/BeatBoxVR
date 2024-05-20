using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public static TutorialManager instance;

    [SerializeField] SelectedUI[] tutorialSelectedUIs;
    [SerializeField] SelectedUI[] bpmSelectedUIs;

    public NotationLoad[] tutorialNotationLoads;
    public TutorialChangeSpeed[] changeSpeeds;

    public TutorialIndicators[] indicators;
    private void Awake()
    {
        changeSpeeds = FindObjectsOfType<TutorialChangeSpeed>();
        if (instance == null)
        {
            instance = this;
        }
    }

    public bool playing = false;
    public void OnClickedPlay()
    {
        if (playing)
        {
            Time.timeScale = 1;
            return;
        }
        
        playing = true;

        string tutorialName = null;


        for (int i = 0; i < tutorialSelectedUIs.Length; i++)
        {
            if (tutorialSelectedUIs[i].active)
            {
                tutorialName = tutorialSelectedUIs[i].GetComponent<TutorialBtn>().tutorialName;
                break;
            }
        }

        if(tutorialName == null)
        {
            Debug.Log("nothing pressed");
            return;
        }

        string bpmName = null;

        for (int i = 0; i < bpmSelectedUIs.Length; i++)
        {
            if (bpmSelectedUIs[i].active)
            {
                bpmName = bpmSelectedUIs[i].GetComponent<BPMBtn>().BPMName;
                break;
            }
        }

        if (bpmName == null)
        {
            Debug.Log("nothing pressed");
            return;
        }
        for (int i = 0;i < tutorialNotationLoads.Length;i++)
        {
            if (tutorialName == tutorialNotationLoads[i].key)
            {
                tutorialNotationLoads[i].ChangeBeat();
            }
        }
        for (int i = 0; i < changeSpeeds.Length; i++)
        {
            if (bpmName == changeSpeeds[i].key)
            {
                changeSpeeds[i].AssignSpeed();
            }
        }

    }
    public void OnClickedStop()
    {
        Time.timeScale = 0;
        
        //indicators.StopNote();
    }

    public void TutorialClose()
    {
        Time.timeScale = 1;
        playing = false;
    }

    public void StartTutorialMode()
    {
        for (int i = 0; i < tutorialSelectedUIs.Length; i++)
        {
            tutorialSelectedUIs[i].OnEnable();
        }
        for (int i = 0; i < bpmSelectedUIs.Length; i++)
        {
            bpmSelectedUIs[i].OnEnable();
        }

        playing = false;
    }
}
