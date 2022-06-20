using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;

public abstract class InputHandler : ScriptableObject
{
    public InputDeviceCharacteristics LeftRight;
    // public abstract bool AssignController(InputDevice device);
    public abstract void UpdateValue();
    protected InputDevice device;
    public bool AssignController(InputDevice device)
    {
        if (device.isValid == false)
        {
            Debug.LogError("Invalid Device");
            return false;
        }

        if (device.characteristics.HasFlag(LeftRight))
        {
            this.device = device;
            return true;
        }

        Debug.LogError("Characteristic not match");
        return false;
    }
    
}
