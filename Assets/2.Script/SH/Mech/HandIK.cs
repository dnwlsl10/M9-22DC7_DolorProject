#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using RootMotion.FinalIK;
public class HandIK : MonoBehaviour, IInitialize
{
    [ContextMenu("Switch Movement Method")]
    void ChangeMethod() => vrController.UseTransformToMove = !vrController.UseTransformToMove;

    [ContextMenu("Find Ref")]
    public void Reset()
    {
#if UNITY_EDITOR
        if (vrController == null) vrController = new VRMap();
        
        vrController.vrOrigin = transform.root.GetComponentInChildren<Camera>(true)?.transform;
        Utility.GetBoneTransform(transform.root, HumanBodyBones.Head, out vrController.rigOrigin);
        Transform xrOrigin = vrController.vrOrigin?.parent;
        
        if (gameObject.name.Contains("Left") || gameObject.name.Contains("left"))
        {
            vrController.vrTarget = Utility.FindChildContainsName(xrOrigin, new string[]{"Left", "left"});
            vrController.rigTarget = Utility.FindChildMatchName(transform.root, "LeftHandTarget");
            vrController.controller = XRNode.LeftHand;

            if (vrController.trackingRotationOffset == Vector3.zero)
                vrController.trackingRotationOffset = new Vector3(88.3f, 21.1f, -0.82f);
        }
        else if (gameObject.name.Contains("Right") || gameObject.name.Contains("right"))
        {
            vrController.vrTarget = Utility.FindChildContainsName(xrOrigin, new string[]{"Right", "right"});
            vrController.rigTarget = Utility.FindChildMatchName(transform.root, "RightHandTarget");
            vrController.controller = XRNode.RightHand;

            if (vrController.trackingRotationOffset == Vector3.zero)
                vrController.trackingRotationOffset = new Vector3(88.3f, 21.1f, -0.82f);
        }
        
        // swivel : -40, WTP : 0, 1, 0, PTT : 0, 0, -1
        vrController.scale = 5.5f;
        vrController.rb = vrController.rigTarget.GetComponent<Rigidbody>();
#endif
    }


    [System.Serializable]
    public class VRMap
    {
        [Tooltip("XR Rig's Head Transform")]
        public Transform vrOrigin;
        [Tooltip("Robot's Head Transform")]
        public Transform rigOrigin;
        [Tooltip("XR Rig's Hand Transform")]
        public Transform vrTarget;
        [Tooltip("Robot's Hand Transform")]
        public Transform rigTarget;

        public XRNode controller;

        public Vector3 trackingPositionOffset;
        public Vector3 trackingRotationOffset;
        public Rigidbody rb;
        bool useTransformToMove;
        public bool UseTransformToMove {
            get {return useTransformToMove;} 
            set
            {
                useTransformToMove = value;
                rb.isKinematic = value;
                rb.GetComponent<Collider>().enabled = !value;

                print("Now Using " + (value ? "Transform" : "RigidBody"));
            }
        }
        [Tooltip("Scale multiplied for calculate Robot's hand position")]
        public float scale = 1;
        [Range(0, 1), Tooltip("Only applied when using Rigidbody movement system")]
        public float speedMultiplier = 1;

        public void MapLocal()
        {
            if (scale == 1)
                rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
            else
            {
                Vector3 dir = vrTarget.position - vrOrigin.position;
                Vector3 position = rigOrigin.position + dir * scale;
                if (useTransformToMove)
                    rigTarget.position = position;
                else
                {
                    rb.velocity = (position - rigTarget.position) / Time.fixedDeltaTime * speedMultiplier;
                }
            }
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }

    public VRMap vrController;
    [Tooltip("hand mesh of pilot in cockpit")]
    public Renderer characterHandMesh;
    bool isLeft;
    private void Awake()
    {
        isLeft = vrController.controller == XRNode.LeftHand;
    }

    private void FixedUpdate() 
    {
        vrController.MapLocal();
    }

#if test
    IEnumerator Start() {
        if (transform.root.GetComponent<Photon.Pun.PhotonView>().Mine == false || Utility.isVRConnected)
            yield break;

        Debug.LogWarning("Hand IK is in testMode");

        if (vrController.vrTarget.TryGetComponent<ActionBasedController>(out var abc) == false)
        {
            abc = vrController.vrTarget.gameObject.AddComponent<ActionBasedController>();
            abc.updateTrackingType = XRBaseController.UpdateType.UpdateAndBeforeRender;
            abc.enableInputTracking = true;
            
            var iam = vrController.vrTarget.GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Inputs.InputActionManager>();
            var asset = iam?.actionAssets[0];
            var map = asset?.actionMaps[(int)(isLeft ? ActionMap.XRI_LeftHand : ActionMap.XRI_RightHand)];

            abc.positionAction = new InputActionProperty(map?.FindAction("Position"));
            abc.rotationAction = new InputActionProperty(map?.FindAction("Rotation"));
            abc.trackingStateAction = new InputActionProperty(map?.FindAction("Tracking State"));

            UnityEngine.InputSystem.XR.TrackedPoseDriver tpd = null;
            if (!isLeft)
            {
                if (vrController.vrOrigin.TryGetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>(out tpd) == false)
                {
                    tpd = vrController.vrOrigin.gameObject.AddComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>();
                }

                tpd.trackingType = UnityEngine.InputSystem.XR.TrackedPoseDriver.TrackingType.RotationAndPosition;
                tpd.updateType = UnityEngine.InputSystem.XR.TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;
                map = asset?.actionMaps[(int)ActionMap.XRI_Head];
                tpd.positionInput = new InputActionProperty(map?.FindAction("Position"));
                tpd.rotationInput = new InputActionProperty(map?.FindAction("Rotation"));
            }

            var v = GameObject.FindObjectOfType<UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation.XRDeviceSimulator>();
            GameObject simulator = v != null ? simulator = v.gameObject : simulator = Instantiate(Resources.Load<GameObject>("TMP/XR Device Simulator"));
            

            abc.enabled = false;
            yield return null;
            iam.enabled = false;
            yield return null;

            if (!isLeft) 
            {
                tpd.enabled = false;
                yield return null;
                simulator.SetActive(false);
            }

            yield return new WaitForSeconds(0.1f);

            iam.enabled = true;
            yield return null;
            abc.enabled = true;
            yield return null;

            if (!isLeft) 
            {
                tpd.enabled = true;
                yield return null;
                simulator.SetActive(true);
            }
        }
    }
#endif
}
