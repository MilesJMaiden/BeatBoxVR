using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButtons> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHovered;
    public Sprite tabSelected;
    public TabButtons selectedTab;

    public void Subscribe(TabButtons button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButtons>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtons button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHovered;
        }
    }

    public void OnTabExit(TabButtons button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtons button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabSelected;
    }

    public void ResetTabs()
    {
        foreach (TabButtons button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
