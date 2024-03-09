using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Note Interaction")]
    public ScoreZone scoreZone; // Reference to the ScoreZone component

    private XRIDefaultInputActions inputActions;
    private InputAction leftJoystickAction;
    private InputAction rightJoystickAction;

    [Header("Game Control")]
    public GameManager gameManager;

    [Header("Sound and VFX")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameObject hiHatVFXPrefab;
    [SerializeField] private GameObject kickDrumVFXPrefab;
    [SerializeField] private Transform hiHatTransform;
    [SerializeField] private Transform kickDrumTransform;
    [SerializeField] private float vfxLifetime = 2.0f;

    [Header("Volume Adjustment")]
    public PlayAlongDetailLoader loader; // Assign this in the Inspector
    private float currentAudioAdjustments;

    [Header("Animation for Kick Drum and HiHat")]
    public GameObject kickDrum;
    public GameObject hiHat;
    private void Awake()
    {
        // Initialize input actions
        inputActions = new XRIDefaultInputActions();
        // Define input actions for left and right controllers
        leftJoystickAction = new InputAction("AdjustRebalancedTrackVolume", binding: "<XRController>{LeftHand}/primary2DAxis");
        rightJoystickAction = new InputAction("AdjustDrumTrackVolume", binding: "<XRController>{RightHand}/primary2DAxis");


        // bindings for pausing the game
        inputActions.XRILeftHand.PauseGame.performed += _ => gameManager.TogglePauseGame();
        inputActions.XRIRightHand.PauseGame.performed += _ => gameManager.TogglePauseGame();

        inputActions.XRILeftHand.PlayHiHatAction.performed += ctx => PlayHiHat();
        inputActions.XRIRightHand.PlayKickDrumAction.performed += ctx => PlayKickDrum();

        inputActions.XRILeftHand.AdjustVolumeRedux.performed += ctx => AdjustVolumeRedux(ctx.ReadValue<Vector2>(), true);
        inputActions.XRIRightHand.AdjustVolumeRedux.performed += ctx => AdjustVolumeRedux(ctx.ReadValue<Vector2>(), false);

        // Assign callbacks for input events
        leftJoystickAction.performed += ctx => AdjustRebalancedTrackVolume(ctx.ReadValue<Vector2>());
        rightJoystickAction.performed += ctx => AdjustDrumTrackVolume(ctx.ReadValue<Vector2>());


    }

    private void OnEnable()
    {
        // Enable the input action maps
        inputActions.XRILeftHandInteraction.Enable();
        inputActions.XRIRightHandInteraction.Enable();

        inputActions.XRILeftHand.Enable();
        inputActions.XRIRightHand.Enable();


        leftJoystickAction.Enable();
        rightJoystickAction.Enable();



        // Assign callbacks for input events
        //leftJoystickAction.performed += ctx => AdjustRebalancedTrackVolume(ctx.ReadValue<Vector2>());
        //rightJoystickAction.performed += ctx => AdjustDrumTrackVolume(ctx.ReadValue<Vector2>());

    }

    private void OnDisable()
    {
        // Disable the input action map when the script is disabled
        inputActions.XRILeftHandInteraction.Disable();
        inputActions.XRIRightHandInteraction.Disable();

        inputActions.XRILeftHand.Disable();
        inputActions.XRIRightHand.Disable();

        leftJoystickAction.Disable();
        rightJoystickAction.Disable();
    }

    private void TogglePauseGame()
    {
        // Directly call the GameManager's TogglePauseGame method
        if (gameManager != null)
        {
            gameManager.TogglePauseGame();
        }
        else
        {
            Debug.LogError("GameManager reference is not set in the Player script.");
        }
    }

    private void PlayHiHat()
    {
        // Play HiHat sound and instantiate VFX
        soundManager.PlaySound("HiHat", hiHatTransform.position, 1.0f);
        InstantiateVFX(hiHatVFXPrefab, hiHatTransform.position);

        // Play HiHat animation
        StartCoroutine(AnimateHiHat());

        // Attempt to hit a HiHat note in the score zone
        scoreZone.AttemptToHitNoteWithTag("HiHat");
    }

    private void PlayKickDrum()
    {
        // Play Kick Drum sound and instantiate VFX
        soundManager.PlaySound("KickDrum", kickDrumTransform.position, 1.0f);
        InstantiateVFX(kickDrumVFXPrefab, kickDrumTransform.position);

        // Play Kick Drum animation
        StartCoroutine(AnimateKickDrum());

        // Attempt to hit a Kick Drum note in the score zone
        scoreZone.AttemptToHitNoteWithTag("KickDrum");
    }

    // This method is correctly set up to attempt to hit a note.
    private void TryHitNote(string noteTag)
    {
        scoreZone.AttemptToHitNoteWithTag(noteTag);
    }

    private IEnumerator AnimateHiHat()
    {
        Animator hiHatAnimator = hiHat.GetComponent<Animator>();
        if (hiHatAnimator != null)
        {
            hiHatAnimator.SetBool("isAnimating", true);

        }
        yield return new WaitForSeconds(0.2f);

        hiHatAnimator.SetBool("isAnimating", false);
    }
    private IEnumerator AnimateKickDrum()
    {
        Animator kickDrumAnimator = kickDrum.GetComponent<Animator>();
        if (kickDrumAnimator != null)
        {
            kickDrumAnimator.SetBool("isAnimating", true);

        }
        yield return new WaitForSeconds(0.2f);

        kickDrumAnimator.SetBool("isAnimating", false);
    }

    private void InstantiateVFX(GameObject vfxPrefab, Vector3 position)
    {
        // Instantiate the VFX prefab and destroy it after vfxLifetime
        var vfxInstance = Instantiate(vfxPrefab, position, Quaternion.identity);
        Destroy(vfxInstance, vfxLifetime);
    }

    private void AdjustRebalancedTrackVolume (Vector2 joystickInput)
    {
        Debug.Log("Left Joystick position: " + joystickInput);
        currentAudioAdjustments = (joystickInput.y * Time.deltaTime) / 2; // Consider scaling this value

        if (currentAudioAdjustments != 0)
        {
            loader.currBalancedTrack.volume += currentAudioAdjustments;
        }


    }

    private void AdjustDrumTrackVolume (Vector2 joystickInput)
    {
        Debug.Log("Right Joystick position: " + joystickInput);
        currentAudioAdjustments = (joystickInput.y * Time.deltaTime) / 2; // Consider scaling this value
        if (currentAudioAdjustments != 0)
        {
            loader.currDrumTrack.volume += currentAudioAdjustments;
        }
    }



    private void AdjustVolumeRedux(Vector2 joystickInput, bool isLeftController)
    {
        if (loader == null) return;

        Debug.Log($"{(isLeftController ? "Left" : "Right")} Joystick: {joystickInput}");
        //adjustment = joystickInput.y * Time.deltaTime;
        currentAudioAdjustments = (joystickInput.y * Time.deltaTime) / 2; // Consider scaling this value

        if (joystickInput.y != 0 && isLeftController)
        {
            loader.currBalancedTrack.volume += currentAudioAdjustments;
        } else if (joystickInput.y != 0 && !isLeftController)
        {
            loader.currDrumTrack.volume += currentAudioAdjustments;
        }
    }
}
