using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public abstract class InputHandler : ScriptableObject
{
    public abstract void UpdateValue(ref InputDevice device);
}
