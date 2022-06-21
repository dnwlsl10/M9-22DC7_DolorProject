using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
    public List<InputHandler> leftControllerInputList = new List<InputHandler>();
    public List<InputHandler> rightControllerInputList = new List<InputHandler>();

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
                if (isLeft) 
                {
                    leftController = devices[0];
                    foreach(var inputHandler in leftControllerInputList)
                        inputHandler.AssignController(leftController);
                }
                else 
                {
                    rightController = devices[0];
                    foreach(var inputHandler in rightControllerInputList)
                        inputHandler.AssignController(rightController);
                }

                devices.Clear();
                break;
            }
            
            yield return null;
        }
    }

    private void Update() 
    {
        UpdateInput();
    }

    void UpdateInput()
    {
        if (rightController.isValid)
            foreach(var button in ( rightControllerInputList))
                button.UpdateValue();
        if (leftController.isValid)
            foreach(var button in ( leftControllerInputList))
                button.UpdateValue();
    }
}
