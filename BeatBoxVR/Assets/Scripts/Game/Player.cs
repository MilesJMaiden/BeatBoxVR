using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private XRIDefaultInputActions inputActions;

    [Header("Game Control")]
    public GamePauseController gamePauseController;
    public UIController uiController;

    [Header("Sound and VFX")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameObject hiHatVFXPrefab;
    [SerializeField] private GameObject kickDrumVFXPrefab;
    [SerializeField] private Transform hiHatTransform;
    [SerializeField] private Transform kickDrumTransform;
    [SerializeField] private float vfxLifetime = 2.0f;

    [Header("Volume Adjustment")]
    public PlayAlongDetailLoader loader; // Assign this in the Inspector


    private void Awake()
    {
        // Initialize input actions
        inputActions = new XRIDefaultInputActions();

        // bindings for pausing the game
        inputActions.XRILeftHand.PauseGame.performed += _ => TogglePauseGame();
        inputActions.XRIRightHand.PauseGame.performed += _ => TogglePauseGame();

        inputActions.XRILeftHand.PlayHiHatAction.performed += ctx => PlayHiHat();
        inputActions.XRIRightHand.PlayKickDrumAction.performed += ctx => PlayKickDrum();

        inputActions.XRILeftHand.AdjustVolume.performed += ctx => AdjustVolume(ctx.ReadValue<Vector2>(), true);
        inputActions.XRIRightHand.AdjustVolume.performed += ctx => AdjustVolume(ctx.ReadValue<Vector2>(), false);
    }

    private void OnEnable()
    {
        // Enable the input action maps
        inputActions.XRILeftHandInteraction.Enable();
        inputActions.XRIRightHandInteraction.Enable();
    }

    private void OnDisable()
    {
        // Disable the input action map when the script is disabled
        inputActions.XRILeftHandInteraction.Disable();
        inputActions.XRIRightHandInteraction.Disable();
    }

    private void TogglePauseGame()
    {
        if (Time.timeScale == 0)
        {
            // Unpause the game
            gamePauseController.UnpauseGameWithCountdown();
            uiController.ToggleMenu(false);
        }
        else
        {
            // Pause the game
            gamePauseController.PauseGame();
            uiController.ToggleMenu(true);
        }
    }

    private void PlayHiHat()
    {
        // Play HiHat sound and instantiate VFX
        soundManager.PlaySound("HiHat", hiHatTransform.position, 1.0f);
        InstantiateVFX(hiHatVFXPrefab, hiHatTransform.position);
    }

    private void PlayKickDrum()
    {
        // Play Kick Drum sound and instantiate VFX
        soundManager.PlaySound("KickDrum", kickDrumTransform.position, 1.0f);
        InstantiateVFX(kickDrumVFXPrefab, kickDrumTransform.position);
    }

    private void InstantiateVFX(GameObject vfxPrefab, Vector3 position)
    {
        // Instantiate the VFX prefab and destroy it after vfxLifetime
        var vfxInstance = Instantiate(vfxPrefab, position, Quaternion.identity);
        Destroy(vfxInstance, vfxLifetime);
    }

    private void AdjustVolume(Vector2 joystickInput, bool isLeftController)
    {
        if (loader == null) return;

        Debug.Log($"{(isLeftController ? "Left" : "Right")} Joystick: {joystickInput}");

        if (joystickInput.y != 0)
        {
            float adjustment = joystickInput.y * Time.deltaTime; // Consider scaling this value
            loader.currBalancedTrack.volume += adjustment;
            loader.currDrumTrack.volume += adjustment;
        }
    }
}
