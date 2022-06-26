//#define SimulateMode
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
        if (vrController == null) vrController = new VRMap();
        
        vrController.vrOrigin = transform.root.GetComponentInChildren<Camera>(true)?.transform;
        Utility.GetBoneTransform(transform.root, HumanBodyBones.Head, out vrController.rigOrigin);
        Transform xrOrigin = vrController.vrOrigin?.parent;
        
        if (gameObject.name.Contains("Left") || gameObject.name.Contains("left"))
        {
            vrController.vrTarget = Utility.FindChildContainsName(xrOrigin, new string[]{"Left", "left"});
            vrController.rigTarget = Utility.FindChildMatchName(transform.root, "LeftHandTarget");
            vrController.controller = XRNode.LeftHand;
        }
        else if (gameObject.name.Contains("Right") || gameObject.name.Contains("right"))
        {
            vrController.vrTarget = Utility.FindChildContainsName(xrOrigin, new string[]{"Right", "right"});
            vrController.rigTarget = Utility.FindChildMatchName(transform.root, "RightHandTarget");
            vrController.controller = XRNode.RightHand;
        }

        vrController.scale = 4;
        vrController.rb = vrController.rigTarget.GetComponent<Rigidbody>();
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

        public Transform CreateLocalTarget()
        {
            GameObject go = new GameObject(controller.ToString() + "Target");
            go.transform.position = vrOrigin.transform.position;

            rb = go.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.isKinematic = false;
            rb.useGravity = false;

            SphereCollider collider = go.AddComponent<SphereCollider>();
            // collider.isTrigger = true;
            collider.radius = 0.4f;
            collider.center = Vector3.right * 0.5f * (controller == XRNode.RightHand ? 1 : -1);

            rigTarget = go.transform;
            return rigTarget;
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
}
