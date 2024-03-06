using UnityEngine;
using TMPro; // For the TextMeshProUGUI
using UnityEngine.UI; // For the Image component

public class EnvironmentManager : MonoBehaviour
{
    [System.Serializable]
    public class EnvironmentSetting
    {
        public string name;
        public Cubemap skybox;
        public GameObject environmentPrefab; // Assume these are pre-placed in the scene
        public Sprite environmentImage; // Sprite for the environment to display in the UI
    }

    public EnvironmentSetting[] environmentSettings;
    public Material skyboxMaterial;
    public TextMeshProUGUI environmentNameText; // Reference to the TextMeshProUGUI component for displaying the environment name
    public Image environmentImageUI; // Reference to the Image component where the environment image will be displayed

    private int currentEnvironmentIndex = -1; // Initialize to an invalid index to ensure the first set is applied

    private void Start()
    {
        // Set the initial environment to the first one
        SetEnvironment(0);
    }

    void Update()
    {
        // Example key inputs to change environment
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextEnvironment();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousEnvironment();
        }
    }

    void SetEnvironment(int index)
    {
        if (index >= 0 && index < environmentSettings.Length)
        {
            // Update the skybox material
            if (skyboxMaterial != null && environmentSettings[index].skybox != null)
            {
                skyboxMaterial.SetTexture("_Tex", environmentSettings[index].skybox);
                DynamicGI.UpdateEnvironment();
            }

            // Disable the currently active environment object
            if (currentEnvironmentIndex >= 0)
            {
                environmentSettings[currentEnvironmentIndex].environmentPrefab.SetActive(false);
            }

            // Enable the new environment object
            environmentSettings[index].environmentPrefab.SetActive(true);

            // Update the TextMeshProUGUI with the name of the current environment
            if (environmentNameText != null)
            {
                environmentNameText.text = environmentSettings[index].name;
            }

            // Update the Image UI component with the sprite of the current environment
            if (environmentImageUI != null && environmentSettings[index].environmentImage != null)
            {
                environmentImageUI.sprite = environmentSettings[index].environmentImage;
            }

            // Update the current environment index
            currentEnvironmentIndex = index;
        }
    }

    public void NextEnvironment()
    {
        int nextIndex = (currentEnvironmentIndex + 1) % environmentSettings.Length;
        SetEnvironment(nextIndex);
    }

    public void PreviousEnvironment()
    {
        int prevIndex = (currentEnvironmentIndex - 1 + environmentSettings.Length) % environmentSettings.Length;
        SetEnvironment(prevIndex);
    }
}
