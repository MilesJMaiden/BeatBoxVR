using UnityEngine;
using UnityEngine.UI;

public class VFXControl : MonoBehaviour
{
    public Drumstick leftDrumstick; // Reference to the Left Drumstick script
    public Drumstick rightDrumstick; // Reference to the Right Drumstick script
    public Toggle vfxToggle; // Reference to the Toggle component

    void Start()
    {
        if (vfxToggle != null)
        {
            vfxToggle.onValueChanged.AddListener(ToggleVFX);
        }
    }

    public void ToggleVFX(bool toggleStatus)
    {
        // Set the instantiateVFX bool in both Drumstick scripts based on the Toggle's status
        if (leftDrumstick != null)
        {
            leftDrumstick.instantiateVFX = toggleStatus;
        }
        if (rightDrumstick != null)
        {
            rightDrumstick.instantiateVFX = toggleStatus;
        }
    }
}
