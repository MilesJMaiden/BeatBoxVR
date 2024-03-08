using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedUIGroup : MonoBehaviour
{
    [SerializeField] SelectedUI[] selectedUIs;

    private void Awake()
    {
        selectedUIs = GetComponentsInChildren<SelectedUI>();
    }

    public void Selected(SelectedUI ui)
    {
        for(int i = 0; i < selectedUIs.Length; i++)
        {
            selectedUIs[i].Selected(ui);
        }
    }
}
