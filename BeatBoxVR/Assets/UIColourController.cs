using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class UIColourController : MonoBehaviour
{
    public UnityEngine.UI.Image targetImage;
    public Color green;
    public Color red;
    public Color blue;
    public Color pink;

    public void changeMonochrome()
    {
        targetImage.gameObject.SetActive(false);
    }
    public void changeGreen()
    {
        if (!targetImage.gameObject.activeSelf)
        {
            targetImage.gameObject.SetActive(true); // Ensure it's active before changing color
        }
        targetImage.color = green;
    }
    public void changeRed()
    {
        if (!targetImage.gameObject.activeSelf)
        {
            targetImage.gameObject.SetActive(true); // Ensure it's active before changing color
        }
        targetImage.color = red;
    }
    public void changeBlue()
    {
        if (!targetImage.gameObject.activeSelf)
        {
            targetImage.gameObject.SetActive(true); // Ensure it's active before changing color
        }
        targetImage.color = blue;
    }
    public void changePink()
    {
        if (!targetImage.gameObject.activeSelf)
        {
            targetImage.gameObject.SetActive(true); // Ensure it's active before changing color
        }
        targetImage.color = pink;
    }
}
