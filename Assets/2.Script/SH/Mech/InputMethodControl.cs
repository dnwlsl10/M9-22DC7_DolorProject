using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMethodControl : MonoBehaviour
{
    public GameObject actionBaseXR;
    public GameObject deviceBaseXR;
    public bool useDeviceBaseInput;


    private void Awake() {
        (useDeviceBaseInput ? deviceBaseXR : actionBaseXR).SetActive(true);
    }
}
