using UnityEngine;

public class DrumCustomizationManager : MonoBehaviour
{
    [Header("Material Customization")]
    public Material drumMaterial; // Shared material for all drums
    private Color defaultColor; // To store the default color of the drum material

    [Header("Texture Options")]
    public Texture textureRock;
    public Texture textureElectronic;
    public Texture textureJazz;
    public Texture texturePop;

    private void Awake()
    {
        // Store the default color of the material
        defaultColor = drumMaterial.color;
    }

    // Method to apply the "Rock" texture
    public void PassInTextureRock()
    {
        SetTexture(textureRock);
    }

    // Method to apply the "Electronic" texture
    public void PassInTextureElectronic()
    {
        SetTexture(textureElectronic);
    }

    // Method to apply the "Jazz" texture
    public void PassInTextureJazz()
    {
        SetTexture(textureJazz);
    }

    // Method to apply the "Pop" texture
    public void PassInTexturePop()
    {
        SetTexture(texturePop);
    }

    // Helper method to set the texture and ensure the color is set to white
    private void SetTexture(Texture texture)
    {
        if (texture != null)
        {
            drumMaterial.mainTexture = texture;
            drumMaterial.color = Color.white; // Ensure the color is white
        }
        else
        {
            // Fallback to default color if texture is null
            RevertToDefaultColor();
        }
    }

    // Method to revert to the default color of the drum material
    public void RevertToDefaultColor()
    {
        drumMaterial.color = defaultColor;
        drumMaterial.mainTexture = null; // Ensure no texture is applied
    }

    // Helper method to set the color and remove any applied texture
    private void SetColor(Color color)
    {
        drumMaterial.color = color;
        drumMaterial.mainTexture = null; // Remove any applied texture
    }

    // Method to set drum color to Red
    public void PassColorRed()
    {
        SetColor(Color.red);
    }

    // Method to set drum color to Green
    public void PassColorGreen()
    {
        SetColor(Color.green);
    }

    // Method to set drum color to Blue
    public void PassColorBlue()
    {
        SetColor(Color.blue);
    }
}
