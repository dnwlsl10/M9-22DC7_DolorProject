using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using System;

public class InputDeviceValue : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField]
    private InputDevice targetDevice;
    public static UnityEvent OnTriggerDown;
    public static event Action OnTriggerUP;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(2.0f);
        List<InputDevice> devices = new List<InputDevice>();
      
        while (devices.Count < 0)
        {
            InputDevices.GetDevices(devices);
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
            foreach (var item in devices)
            {
                Debug.Log(item.name + item.characteristics);
            }

            if (devices.Count > 0)
            {
                targetDevice = devices[0];
                yield break;
            }

            yield return null;
        }
    }

    bool isPress;

    void OnTigger()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerButtonValue);

        /*if(triggerButtonValue && !isPress)
        {
            isPress = true;
            OnTriggerDown?.Invoke();
        }
        if (!triggerButtonValue && isPress)
        {
            OnTriggerUP?.Invoke();
            isPress = triggerButtonValue;
        }*/


        if (isPress != triggerButtonValue)
        {
            isPress = triggerButtonValue;
            //(isPress ? OnTriggerDown : OnTriggerUP)?.Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {

        OnTigger();
        // Y B
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue) Debug.Log("Pressing Primary Button");

        //X A 
        targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
        if (secondaryButtonValue) Debug.Log("Pressing Primary Button");

        //Trigger
        targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
        if (triggerValue > 0.1f) Debug.Log("Trigger pressed" + triggerValue);


        //joyStick
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue);
        if (primary2DAxisValue != Vector2.zero) Debug.Log("Primary Touchpad" + primary2DAxisValue);

        //grip
        targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue);
        if (gripValue > 0.1f) Debug.Log("Grip pressed" + gripValue);

        //grip press
        targetDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButtonValue);
        if (gripButtonValue) Debug.Log("Pressing GirpButton");

        //trigger Button


        //joyStick press
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool primary2DAxisClick);
        if (primary2DAxisClick) Debug.Log("Pressing JoyStick");

        //joyStick Touch
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool primary2DAxisTouch);
        if (primary2DAxisTouch) Debug.Log("Touch JoyStick");

    }

}
