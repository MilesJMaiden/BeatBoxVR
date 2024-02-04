using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    // Array to hold the Cubemap textures you want to switch between
    public Cubemap[] skyboxCubemaps;

    // The generic skybox material to be modified at runtime
    public Material skyboxMaterial;

    private void Awake()
    {
        // Ensure the skybox material is set to the RenderSettings
        if (skyboxMaterial != null)
            RenderSettings.skybox = skyboxMaterial;
    }

    void Update()
    {
        // Check for key presses and switch skybox Cubemaps accordingly
        for (int i = 0; i < skyboxCubemaps.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSkyboxCubemap(i);
                break; // Exit the loop once a key press is detected and handled
            }
        }

        // Update the skybox material changes in the editor
        DynamicGI.UpdateEnvironment();
    }

    void SetSkyboxCubemap(int index)
    {
        if (index < skyboxCubemaps.Length && skyboxMaterial != null)
        {
            skyboxMaterial.SetTexture("_Tex", skyboxCubemaps[index]);
        }
    }
}
