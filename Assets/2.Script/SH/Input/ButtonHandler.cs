using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable, CreateAssetMenu(fileName = "NewButtonHandler")]
public class ButtonHandler : InputHandler
{
    // public InputDeviceCharacteristics character
    public InputHelpers.Button button;
    public delegate void EventContainer();
    public event EventContainer OnButtonUp;
    public event EventContainer OnButtonDown;
    public bool isPress {get; private set;}

    public override void UpdateValue(ref InputDevice device)
    {
        if (device.IsPressed(button, out bool tmp))
        {
            if (isPress != tmp)
            {
                isPress = tmp;
                (isPress ? OnButtonDown : OnButtonUp)?.Invoke();
            }
        }
    }
}
