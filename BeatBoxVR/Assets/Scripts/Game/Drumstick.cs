using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class Drumstick : MonoBehaviour
{
    [SerializeField] private Player player;

    private SoundManager soundManager;
    
    public GameObject whiteSparkVFXPrefab;
    public GameObject yellowSparkVFXPrefab;
    public GameObject redSparkVFXPrefab;
    
    public float vfxLifetime = 1.4f;
    private const float MaxVelocity = 10f;

    public Transform tipTransform;
    private Vector3 previousTipPosition;
    private Vector3 tipMovementDirection;
    private float tipVelocity;

    public bool instantiateVFX = false; // Flag to control VFX instantiation
    public bool enableHapticFeedback = true; // Flag to control haptic feedback
    public float LastHitVelocity { get; private set; }

    public ScoreZone scoreZone;

    void Start()
    {
        //player = FindObjectOfType<Player>(); // Ensure there's a way to access the Player instance
        soundManager = FindObjectOfType<SoundManager>();
        previousTipPosition = tipTransform.position;
    }

    void Update()
    {
        Vector3 currentTipPosition = tipTransform.position;
        tipMovementDirection = currentTipPosition - previousTipPosition;
        tipVelocity = tipMovementDirection.magnitude / Time.deltaTime;
        previousTipPosition = currentTipPosition;
    }

    // Method to get current velocity
    public float GetCurrentVelocity()
    {
        return Mathf.Clamp(tipVelocity, 0, MaxVelocity);
    }

    void OnTriggerEnter(Collider other)
    {
        if (tipMovementDirection.y < 0) // Check if moving downwards
        {
            float clampedVelocity = GetCurrentVelocity();
            bool isHiHatOpen = player?.GetIsHiHatOpen() ?? false;
            string soundTag = other.tag == "HiHat" ? (isHiHatOpen ? "HiHat Open" : "HiHat Closed") : other.tag;

            soundManager.PlaySound(soundTag, tipTransform.position, clampedVelocity / MaxVelocity, isHiHatOpen);

            if (clampedVelocity > 1 && instantiateVFX)
            {
                string vfxTag = GetVFXTagBasedOnVelocity(clampedVelocity);
                Vector3 hitPoint = other.ClosestPoint(tipTransform.position);
                Quaternion hitRotation = Quaternion.LookRotation(-tipMovementDirection);
                InstantiateVFX(vfxTag, hitPoint, hitRotation, clampedVelocity);
            }

            if (enableHapticFeedback)
            {
                TriggerHapticFeedback(gameObject.tag, 0.1f, Mathf.InverseLerp(0, MaxVelocity, clampedVelocity));
            }

            if (scoreZone != null)
            {
                scoreZone.AttemptToHitNoteWithTag(soundTag);
            }
        }
    }

    private string GetVFXTagBasedOnVelocity(float velocity)
    {
        if (velocity <= 4)
            return "WhiteSpark";
        else if (velocity <= 7)
            return "YellowSpark";
        else
            return "RedSpark";
    }

    // Method to toggle VFX instantiation
    public void ToggleVFXInstantiation(bool isEnabled)
    {
        instantiateVFX = isEnabled;
    }

    public void ToggleHapticFeedback(bool isEnabled)
    {
        enableHapticFeedback = isEnabled;
    }

    private GameObject SelectVFXPrefabBasedOnVelocity(float velocity)
    {
        
        if (velocity <= 4)
            return whiteSparkVFXPrefab;
        else if (velocity <= 7)
            return yellowSparkVFXPrefab;
        else
            return redSparkVFXPrefab;
        
    }

    private void InstantiateVFX(string vfxTag, Vector3 position, Quaternion rotation, float velocity)
    {
        // Spawn the VFX GameObject from the pool with a specified lifetime
        GameObject vfxInstance = VFXPoolManager.Instance.SpawnFromPool(vfxTag, position, rotation, vfxLifetime);
        if (vfxInstance != null)
        {
            // Get the VFXInstance component from the spawned GameObject
            VFXInstance vfxControl = vfxInstance.transform.GetChild(0).GetComponent<VFXInstance>();
            if (vfxControl != null)
            {
                vfxControl.ApplyParticleModifications(velocity);
            }

            StartCoroutine(DisableAndReturnToPool(vfxInstance, vfxLifetime));
        }
    }

    IEnumerator DisableAndReturnToPool(GameObject vfxInstance, float delay)
    {
        yield return new WaitForSeconds(delay);
        vfxInstance.SetActive(false); // Deactivates the instance, making it ready for reuse
    }


    private void ApplyParticleModifications(ParticleSystem particleSystem, float velocity)
    {
        float scaleMultiplier = CalculateScaleMultiplier(velocity);
        var mainModule = particleSystem.main;

        // Adjust start speed and size based on velocity
        mainModule.startSpeed = Mathf.Lerp(0.01f, 1f, Mathf.Pow(scaleMultiplier, 2));
        mainModule.startSize = scaleMultiplier;

        // Adjust emission rate
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTime = Mathf.Lerp(10, 50, scaleMultiplier);

        // Adjust shape length for a more natural effect
        var shapeModule = particleSystem.shape;
        shapeModule.length = Mathf.Lerp(0.005f, 0.1f, Mathf.Pow(scaleMultiplier, 2));
    }

    private float CalculateScaleMultiplier(float velocity)
    {
        // Adjust this method as needed to fit the desired effect
        return Mathf.Clamp((velocity / MaxVelocity), 0.5f, 2.5f);
    }

    private void TriggerHapticFeedback(string drumstickTag, float duration, float strength)
    {
        InputDevice device = GetDeviceByDrumstickTag(drumstickTag);

        if (device.isValid)
        {
            device.SendHapticImpulse(0, strength, duration);
        }
    }

    private InputDevice GetDeviceByDrumstickTag(string tag)
    {
        // This method should be implemented to return the correct InputDevice
        // based on whether the tag is LeftDrumstick or RightDrumstick.

        if (tag == "LeftDrumstick")
        {
            return InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }
        else if (tag == "RightDrumstick")
        {
            return InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        }

        return new InputDevice(); // (Fallback) ideally never used
    }
}