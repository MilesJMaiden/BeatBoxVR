using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUI : MonoBehaviour
{
    public GameObject activeImg;
    public bool active;


    private void OnEnable()
    {
        activeImg.SetActive(false);
        active = false;
    }
    public void Selected(SelectedUI ui)
    {
        // this component selected
        if (this == ui)
        {
            activeImg.SetActive(true);
            active = true;
        }
        else // not selected
        {
            activeImg.SetActive(false);
            active = false;
        }
    }
    public void OnClickedBtn()
    {
        GetComponentInParent<SelectedUIGroup>().Selected(this);
    }
}
