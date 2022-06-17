using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    public ButtonHandler rightGripBtn;
    public ButtonHandler leftGripBtn;

    private void OnEnable() {
        rightGripBtn.OnButtonDown += OnRightGripButtonDown;
        rightGripBtn.OnButtonUp += OnRightGripButtonUp;
        leftGripBtn.OnButtonDown += OnLeftGripButtonDown;
        leftGripBtn.OnButtonUp += OnLeftGripButtonUp;
    }

    private void OnDisable() 
    {
        rightGripBtn.OnButtonDown -= OnRightGripButtonDown;
        rightGripBtn.OnButtonUp -= OnRightGripButtonUp;
        leftGripBtn.OnButtonDown -= OnLeftGripButtonDown;
        leftGripBtn.OnButtonUp -= OnLeftGripButtonUp;
    }

    private void OnRightGripButtonDown() => print("Right Grip Button Pressed");
    private void OnRightGripButtonUp() => print("Right Grip Button Released");

    private void OnLeftGripButtonDown() => print("Left Grip Button Pressed");
    private void OnLeftGripButtonUp() => print("Left Grip Button Released");
}
