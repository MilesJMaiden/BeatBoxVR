using UnityEngine;

public class VFXInstance : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void ApplyParticleModifications(float velocity)
    {
        float scaleMultiplier = CalculateScaleMultiplier(velocity);
        var mainModule = particleSystem.main;
        mainModule.startSpeed = Mathf.Lerp(0.01f, 1f, Mathf.Pow(scaleMultiplier, 2));
        mainModule.startSize = scaleMultiplier;

        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = Mathf.Lerp(10, 100, scaleMultiplier);

        var shapeModule = particleSystem.shape;
        shapeModule.length = Mathf.Lerp(0.005f, 0.2f, Mathf.Pow(scaleMultiplier, 2));
    }

    private float CalculateScaleMultiplier(float velocity)
    {
        // Adjust this method as needed to fit the desired effect
        return Mathf.Clamp((velocity / 10f), 0.5f, 2.5f); // Example assumes MaxVelocity is 10
    }
}
