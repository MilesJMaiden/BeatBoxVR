using UnityEngine;
using UnityEngine.UI; // Include the UI namespace for Button.

public class TabSystem : MonoBehaviour
{
    // Array of buttons for the tabs.
    public Button[] tabs;

    // Array of GameObjects that correspond to each tab.
    public GameObject[] tabPages;

    void Start()
    {
        // Initialize all tabs.
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i; // Local copy for the closure.
            tabs[i].onClick.AddListener(() => OnTabClicked(index));
        }

        // Initialize by showing the first tab active.
        OnTabClicked(0);
    }

    void OnTabClicked(int tabIndex)
    {
        // Loop through all GameObjects and activate the one corresponding to the clicked tab.
        // Deactivate all others.
        for (int i = 0; i < tabPages.Length; i++)
        {
            if (tabPages[i] != null) // Check if the GameObject reference is not null.
            {
                tabPages[i].SetActive(i == tabIndex);
            }
        }
    }
}
