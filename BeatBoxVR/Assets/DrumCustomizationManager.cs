using UnityEngine;

public class DrumCustomizationManager : MonoBehaviour
{
    [Header("Material Customization")]
    public Material drumMaterial; // Shared material for all drums

    [Header("Texture Options")]
    public Texture[] drumTextures; // Array of textures for the drums

    [Header("Color Options")]
    public Color[] drumColors; // Array of colors for the drums

    // Method to change the drum texture
    // This method is intended to be called by UI buttons, passing in the index of the desired texture
    public void ChangeDrumTexture(int textureIndex)
    {
        if (drumTextures != null && textureIndex >= 0 && textureIndex < drumTextures.Length)
        {
            drumMaterial.mainTexture = drumTextures[textureIndex];
        }
        else
        {
            Debug.LogWarning("Invalid texture index or texture array not set.");
        }
    }

    // Method to change the drum color
    // This method is intended to be called by UI buttons, passing in the index of the desired color
    public void ChangeDrumColor(int colorIndex)
    {
        if (drumColors != null && colorIndex >= 0 && colorIndex < drumColors.Length)
        {
            drumMaterial.color = drumColors[colorIndex];
            drumMaterial.mainTexture = null; // Remove any texture, using color only
        }
        else
        {
            Debug.LogWarning("Invalid color index or color array not set.");
        }
    }
}
