using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable, CreateAssetMenu(fileName = "NewButtonHandler")]
public class ButtonHandler
{
    public InputHelpers.Button button;
    public delegate void TMP();
    public static event TMP OnButtonUp;
    // public UnityEvent<XRController> onButtonDown;
    // public UnityEvent<XRController> onButtonUp;
    public bool isPress {get; private set;}

    // public void UpdateValue(ref XRController controller)
    // {
    //     if (controller.inputDevice.IsPressed(button, out bool tmp))
    //     {
    //         if (isPress != tmp)
    //         {
    //             isPress = tmp;
    //             (isPress ? onButtonDown : onButtonUp)?.Invoke(controller);
    //         }
    //     }
    // }

    public void InvokeTest(XRController controller)
    {
        //OnButtonUp?.Invoke(controller);
        //OnButtonDown?.Invoke(controller);

    }
}
