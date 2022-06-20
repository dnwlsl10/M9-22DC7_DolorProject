using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable, CreateAssetMenu(fileName = "NewButtonHandler")]
public class ButtonHandler : InputHandler
{
    public InputHelpers.Button button;
    public delegate void EventContainer();
    public event EventContainer OnButtonUp;
    public event EventContainer OnButtonDown;
    public bool isPress {get; private set;}
    private InputDevice controller;
    bool isValid;

    public override void UpdateValue()
    {
        if (isValid == false)
        {
            Debug.LogWarning("Invalid device " + device.name);
            return;
        }

        if (device.IsPressed(button, out bool tmp))
        {
            if (isPress != tmp)
            {
                isPress = tmp;
                (isPress ? OnButtonDown : OnButtonUp)?.Invoke();
            }
        }
    }

    public bool GetValue(out bool _value)
    {
        _value = isPress;
        return isValid;
    }
}
