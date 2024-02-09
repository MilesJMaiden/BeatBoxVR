using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class JoystickTrackAdjustment : MonoBehaviour
{

    private XRInputData _inputData;
    public PlayAlongDetailLoader loader;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputData._leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 leftJoystick))
        {
            Debug.Log("leftJoystick: " + leftJoystick);

            if (leftJoystick.y != 0)
            {
                loader.currBalancedTrack.volume = loader.currBalancedTrack.volume + leftJoystick.y;
                loader.currDrumTrack.volume = loader.currBalancedTrack.volume + leftJoystick.y;
            }


        }
        if (_inputData._rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 rightJoystick))
        {
            Debug.Log("rightJoystick: " + rightJoystick);

            if (rightJoystick.y != 0)
            {
                loader.currBalancedTrack.volume = loader.currBalancedTrack.volume + rightJoystick.y;
                loader.currDrumTrack.volume = loader.currBalancedTrack.volume + rightJoystick.y;
            }
        }

    }
}
