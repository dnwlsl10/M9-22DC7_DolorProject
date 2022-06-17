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

    void Start() 
    {
        StartCoroutine(FindController(true));
        StartCoroutine(FindController(false));
    }

    IEnumerator FindController(bool isLeft)
    {
        List<InputDevice> devices = new List<InputDevice>();

        while(true)
        {
            InputDeviceCharacteristics ControllerCharacteristics =
                InputDeviceCharacteristics.Controller | (isLeft ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right);

            InputDevices.GetDevicesWithCharacteristics(ControllerCharacteristics, devices);
            if (devices.Count > 0)
            {
                if (isLeft) leftController = devices[0];
                else rightController = devices[0];

                devices.Clear();
                break;
            }
            
            yield return null;
        }
    }

    bool isLeft;
    private void Update() 
    {
        UpdateInput(rightController);
        UpdateInput(leftController);
    }

    void UpdateInput(InputDevice controller)
    {
        if (controller.isValid == false)
        {
            Debug.LogWarning("Invalid controller");
            return;
        }

        if (controller.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            foreach(var button in ( rightControllerInputList))
                button.UpdateValue(ref controller);
        else if (controller.characteristics.HasFlag(InputDeviceCharacteristics.Left))
            foreach(var button in ( leftControllerInputList))
                button.UpdateValue(ref controller);
    }
}
