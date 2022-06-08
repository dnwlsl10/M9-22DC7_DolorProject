using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;




public class InputManager : MonoBehaviour
{
    [ContextMenu("InvokeUpEvent")]
    void InvokeUpEvent()
    {
        foreach (var button in buttons)
        {
            button.InvokeTest(controller);
        }
    }
    public ButtonHandler[] buttons;
    public XRController controller;

    private void Update() 
    {
        //btn.UpdateValue(ref controller);
    }
}
