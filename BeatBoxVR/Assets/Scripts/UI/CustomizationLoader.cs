using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizationLoader : MonoBehaviour
{

    public int currentEnvironmentID = 0;
    public int currentDrumkitSkinID = 0;

    public Button IncEnvironmentButton;
    public Button DecEnvironmentButton;
    public Button IncSkinButton;
    public Button DecSkinButton;


    public TextMeshProUGUI SkinText;
    public TextMeshProUGUI EnvironmentText;
    public Image EnvironmentPreview;
    public Image DrumkitSkinPreview;


    private GameObject currentEnvironment;
    private GameObject currentDrumkitSkin;

    public EnvironmentInfo[] environmentContainer;
    public DrumkitSkinInfo[] skinContainer;


    private void Start()
    {
        currentEnvironment = environmentContainer[0].environmentPrefab;
        currentDrumkitSkin = skinContainer[0].DrumkitPrefab;
    }


    public void increaseEnvironmentID()
    {
        currentEnvironmentID++;
        if (currentEnvironmentID >= environmentContainer.Length)
        {
            currentEnvironmentID = 0;
        }
        //Add transition?
        loadEnvironment(currentEnvironmentID);
    }

    public void decreaseEnvironmentID()
    {
        currentEnvironmentID--;
        if (currentEnvironmentID < 0)
        {
            currentEnvironmentID = environmentContainer.Length - 1;
        }
        //Add transition?
        loadEnvironment(currentEnvironmentID);
    }


    private void loadEnvironment(int environmentID)
    {
        EnvironmentText.text = environmentContainer[environmentID].EnvironmentName;
        EnvironmentPreview.sprite = environmentContainer[environmentID].EnvironmentPreviewSprite;
        currentEnvironment.SetActive(false);
        //environmentContainer[environmentID].environmentPrefab.SetActive(true);
        currentEnvironment = environmentContainer[environmentID].environmentPrefab;
        currentEnvironment.SetActive(true);

    }


    public void increaseSkinID()
    {
        currentDrumkitSkinID++;
        if (currentDrumkitSkinID >= skinContainer.Length)
        {
            currentDrumkitSkinID = 0;
        }
        loadEnvironment(currentDrumkitSkinID);

    }

    public void decreaseSkinID()
    {
        currentDrumkitSkinID--;
        if (currentDrumkitSkinID < 0)
        {
            currentDrumkitSkinID = skinContainer.Length - 1;
        }
        loadDrumkitSkin(currentDrumkitSkinID);
    }


    private void loadDrumkitSkin(int skinID)
    {
        EnvironmentText.text = skinContainer[skinID].DrumkitSkinName ;
        EnvironmentPreview.sprite = skinContainer[skinID].DrumkitSkinPreview;

        currentDrumkitSkin.SetActive(false);
        currentDrumkitSkin = skinContainer[skinID].DrumkitPrefab;
        currentDrumkitSkin.SetActive(true);


    }

    [System.Serializable]
    public struct EnvironmentInfo
    {
        public string EnvironmentName;
        public Sprite EnvironmentPreviewSprite;
        public GameObject environmentPrefab;

    }

    [System.Serializable]
    public struct DrumkitSkinInfo
    {
        public string DrumkitSkinName;
        public Sprite DrumkitSkinPreview;
        public GameObject DrumkitPrefab; //Change to whatever needs to be done to get skins to change.
    }
}
