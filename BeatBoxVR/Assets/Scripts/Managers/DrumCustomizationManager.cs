using UnityEngine;
using TMPro; // Include the TextMeshPro namespace
using UnityEngine.UI; // Include the UI namespace for the Image component

public class DrumCustomizationManager : MonoBehaviour
{
    [System.Serializable]
    public class DrumTextureSet
    {
        public string name;
        public Texture baseMap;
        public Texture specularMap;
        public Texture normalMap;
        public Sprite setPreviewImage; // Sprite for preview in the UI
    }

    [Header("Material Customization")]
    public Material drumMaterial; // Shared material for all drums

    [Header("Texture Sets")]
    public DrumTextureSet[] textureSets;

    [Header("UI Components")]
    public TextMeshProUGUI textureSetNameText; // Reference to the TextMeshProUGUI component
    public Image textureSetPreviewImage; // Reference to the Image component for displaying the set preview

    private int currentTextureSetIndex = 0;

    private void Start()
    {
        ApplyTextureSet(currentTextureSetIndex); // Apply the first texture set by default
    }

    // Method to apply the next texture set
    public void NextTextureSet()
    {
        currentTextureSetIndex = (currentTextureSetIndex + 1) % textureSets.Length;
        ApplyTextureSet(currentTextureSetIndex);
    }

    // Method to apply the previous texture set
    public void PreviousTextureSet()
    {
        currentTextureSetIndex--;
        if (currentTextureSetIndex < 0)
        {
            currentTextureSetIndex = textureSets.Length - 1; // Loop back to the last set
        }
        ApplyTextureSet(currentTextureSetIndex);
    }

    // Helper method to apply texture sets based on the index
    private void ApplyTextureSet(int index)
    {
        DrumTextureSet textureSet = textureSets[index];
        drumMaterial.SetTexture("_MainTex", textureSet.baseMap);
        drumMaterial.SetTexture("_SpecGlossMap", textureSet.specularMap);
        drumMaterial.SetTexture("_BumpMap", textureSet.normalMap);

        // Update the TextMeshProUGUI with the name of the current texture set
        if (textureSetNameText != null)
        {
            textureSetNameText.text = textureSet.name;
        }

        // Update the Image UI component with the sprite of the current texture set
        if (textureSetPreviewImage != null && textureSet.setPreviewImage != null)
        {
            textureSetPreviewImage.sprite = textureSet.setPreviewImage;
        }
    }
}
