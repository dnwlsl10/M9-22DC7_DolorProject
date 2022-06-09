using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

// OpenXR Interaction Profiles - add Oculus Touch Controller Profile , Valve Index Controller Profile
public class HMDInfoManager : MonoBehaviour
{
    public bool useActionBaseInput;
    public GameObject actionBaseXROrigin;
    public GameObject deviceBaseXROrigin;
    public GameObject go;

    private void Awake() 
    {
        go.SetActive(true);
        (!useActionBaseInput ? actionBaseXROrigin : deviceBaseXROrigin).SetActive(false);
    }

}
