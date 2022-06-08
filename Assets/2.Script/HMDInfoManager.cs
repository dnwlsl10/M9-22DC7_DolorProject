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
    public InputActionAsset defaultAction;
    public InputActionReference[] camRef;
    public InputActionReference[] rightControllerRef;
    public InputActionReference[] leftControllerRef;

    private void Awake() 
    {
        StartCoroutine(SwitchToActionBase());
    }

    IEnumerator SwitchToActionBase()
    {
        GameObject origin = GameObject.FindObjectOfType<XROrigin>().gameObject;
        if (origin.TryGetComponent<InputActionManager>(out InputActionManager iam) == false)
            origin.AddComponent<InputActionManager>().actionAssets.Add(defaultAction);

        GameObject cam = Camera.main.gameObject;
        if (cam.TryGetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>(out UnityEngine.InputSystem.XR.TrackedPoseDriver tpd) == false)
            tpd = cam.AddComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>();    
        tpd.positionInput = new InputActionProperty(camRef[0]);
        tpd.rotationInput = new InputActionProperty(camRef[1]);

        if (cam.TryGetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>(out UnityEngine.SpatialTracking.TrackedPoseDriver tpd2))
            Destroy(tpd2);

        XRController[] deviceBaseController =  GameObject.FindObjectsOfType<XRController>();
        foreach (var controller in deviceBaseController)
        {
            if (controller.controllerNode == XRNode.RightHand)
            {
                GameObject go = controller.gameObject;
                Destroy(controller);
                yield return null;
                ActionBasedController abc = go.AddComponent<ActionBasedController>();

                abc.positionAction = new InputActionProperty(rightControllerRef[0]);
                abc.rotationAction = new InputActionProperty(rightControllerRef[1]);
                abc.trackingStateAction = new InputActionProperty(rightControllerRef[2]);
                abc.selectAction = new InputActionProperty(rightControllerRef[3]);
                abc.selectActionValue = new InputActionProperty(rightControllerRef[4]);
                abc.activateAction = new InputActionProperty(rightControllerRef[5]);
                abc.activateActionValue = new InputActionProperty(rightControllerRef[6]);
                abc.uiPressAction = new InputActionProperty(rightControllerRef[7]);
                abc.uiPressActionValue = new InputActionProperty(rightControllerRef[8]);
                abc.hapticDeviceAction = new InputActionProperty(rightControllerRef[9]);
                abc.rotateAnchorAction = new InputActionProperty(rightControllerRef[10]);
                abc.translateAnchorAction = new InputActionProperty(rightControllerRef[11]);
            }
            else if (controller.controllerNode == XRNode.LeftHand)
            {
                GameObject go = controller.gameObject;
                Destroy(controller);
                yield return null;
                ActionBasedController abc = go.AddComponent<ActionBasedController>();

                abc.positionAction = new InputActionProperty(leftControllerRef[0]);
                abc.rotationAction = new InputActionProperty(leftControllerRef[1]);
                abc.trackingStateAction = new InputActionProperty(leftControllerRef[2]);
                abc.selectAction = new InputActionProperty(leftControllerRef[3]);
                abc.selectActionValue = new InputActionProperty(leftControllerRef[4]);
                abc.activateAction = new InputActionProperty(leftControllerRef[5]);
                abc.activateActionValue = new InputActionProperty(leftControllerRef[6]);
                abc.uiPressAction = new InputActionProperty(leftControllerRef[7]);
                abc.uiPressActionValue = new InputActionProperty(leftControllerRef[8]);
                abc.hapticDeviceAction = new InputActionProperty(leftControllerRef[9]);
                abc.rotateAnchorAction = new InputActionProperty(leftControllerRef[10]);
                abc.translateAnchorAction = new InputActionProperty(leftControllerRef[11]);
            }
        }
    }
}
