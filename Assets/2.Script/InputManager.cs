using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    public List<InputHandler> rightControllerInputList = new List<InputHandler>();
    public List<InputHandler> leftControllerInputList = new List<InputHandler>();

    InputDevice rightController;
    InputDevice leftController;

    IEnumerator Start() 
    {
        List<InputDevice> devices = new List<InputDevice>();

        while(true)
        {
            InputDeviceCharacteristics rightControllerCharacteristics =
                InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

            InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
            if (devices.Count > 0)
            {
                rightController = devices[0];
                StartCoroutine(UpdateInputValues(rightController));
                break;
            }
            
            yield return null;
        }

        while(true)
        {
            InputDeviceCharacteristics leftControllerCharacteristics =
                InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;

            InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, devices);
            if (devices.Count > 0)
            {
                leftController = devices[0];
                StartCoroutine(UpdateInputValues(leftController));
                break;
            }

            yield return null;
        }
    }

    IEnumerator UpdateInputValues(InputDevice controller)
    {
        if (controller.characteristics == (InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller))
            while(true)
            {
                foreach(var button in rightControllerInputList)
                    button.UpdateValue(ref controller);

                yield return null;
            }
            
        else if (controller.characteristics == (InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller))
            while(true)
            {
                foreach(var button in leftControllerInputList)
                    button.UpdateValue(ref controller);

                yield return null;
            }
    }
}
