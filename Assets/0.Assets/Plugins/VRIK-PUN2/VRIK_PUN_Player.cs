using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using Photon.Pun;

namespace RootMotion.Demos
{
    /// <summary>
    /// VRIK Player for Photon Unity Networking
    /// </summary>
    public class VRIK_PUN_Player : MonoBehaviourPun, IPunObservable, IPunInstantiateMagicCallback
    {

        #region All

        [Tooltip("Root of the VR camera rig")] public GameObject vrRig;
        [Tooltip("The VRIK component.")] public VRIK ik;

        // NetworkTransforms are network snapshots of Transform position, rotation, velocity and angular velocity
        private NetworkTransform rootNetworkT = new NetworkTransform();
        private NetworkTransform headNetworkT = new NetworkTransform();
        private NetworkTransform leftHandNetworkT = new NetworkTransform();
        private NetworkTransform rightHandNetworkT = new NetworkTransform();

        // Called by Photon when the player is instantiated
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            // Initiation
            if (photonView.IsMine)
            {
                InitiateLocal();
            }
            else
            {
                InitiateRemote();
            }

            name = "VRIK_PUN_Player " + (photonView.IsMine ? "(Local)" : "(Remote)");
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                UpdateLocal();
            }
            else
            {
                UpdateRemote();
            }
        }

        // Sync NetworkTransforms
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // Send NetworkTransform data
                rootNetworkT.Send(stream);
                headNetworkT.Send(stream);
                leftHandNetworkT.Send(stream);
                rightHandNetworkT.Send(stream);
            }
            else
            {
                // Receive NetworkTransform data
                rootNetworkT.Receive(stream);
                headNetworkT.Receive(stream);
                leftHandNetworkT.Receive(stream);
                rightHandNetworkT.Receive(stream);
            }
        }

        #endregion All

        // Code that runs only for local instances of this player
        #region Local

        [LargeHeader("Calibration")]
        
        [Header("Head")]
        [Tooltip("HMD.")] public Transform centerEyeAnchor;
        [Tooltip("Position offset of the camera from the head bone (root space).")] public Vector3 headAnchorPositionOffset;
        [Tooltip("Rotation offset of the camera from the head bone (root space).")] public Vector3 headAnchorRotationOffset;

        [Header("Hands")]
        [Tooltip("Left Hand Controller")] public Transform leftHandAnchor;
        [Tooltip("Right Hand Controller")] public Transform rightHandAnchor;
        [Tooltip("Position offset of the hand controller from the hand bone (controller space).")] public Vector3 handAnchorPositionOffset;
        [Tooltip("Rotation offset of the hand controller from the hand bone (controller space).")] public Vector3 handAnchorRotationOffset;

        [Header("Scale")]
        [Tooltip("Multiplies the scale of the root.")] public float scaleMlp = 1f;
        public bool fixedScale = true;

        [Header("Data stored by Calibration")]
        public VRIKCalibrator.CalibrationData data = new VRIKCalibrator.CalibrationData();

        private void InitiateLocal()
        {
            vrRig.SetActive(true);

            // Calibrate the character
            data = VRIKCalibrator.Calibrate(ik, centerEyeAnchor, leftHandAnchor, rightHandAnchor, headAnchorPositionOffset, headAnchorRotationOffset, handAnchorPositionOffset, handAnchorRotationOffset, scaleMlp);

            if (fixedScale)
            {
                ik.references.root.localScale = scaleMlp * Vector3.one;
                data.scale = scaleMlp;
            }
        }

        private void UpdateLocal()
        {
            // Update IK target velocities (for interpolation)
            rootNetworkT.ReadVelocitiesLocal(ik.references.root);
            headNetworkT.ReadVelocitiesLocal(ik.solver.spine.headTarget);
            leftHandNetworkT.ReadVelocitiesLocal(ik.solver.leftArm.target);
            rightHandNetworkT.ReadVelocitiesLocal(ik.solver.rightArm.target);

            // Update IK target positions/rotations
            rootNetworkT.ReadTransformLocal(ik.references.root);
            headNetworkT.ReadTransformLocal(ik.solver.spine.headTarget);
            leftHandNetworkT.ReadTransformLocal(ik.solver.leftArm.target);
            rightHandNetworkT.ReadTransformLocal(ik.solver.rightArm.target);
        }

        #endregion Local

        // Code that runs only for remote instances of this player
        #region Remote

        [LargeHeader("Remote")]
        [Tooltip("The speed of interpolating remote IK targets.")] public float proxyInterpolationSpeed = 20f;
        [Tooltip("Max interpolation error square magnitude. IK targets snap to latest synced position if current interpolated position is farther than that.")] public float proxyMaxErrorSqrMag = 4f;
        [Tooltip("If assigned, remote instances of this player will use this material.")] public Material remoteMaterialOverride;

        private Transform headIKProxy;
        private Transform leftHandIKProxy;
        private Transform rightHandIKProxy;

        private void InitiateRemote()
        {
            // Remote instance does not have a VR rig, so we use proxies for them. Positions and rotations of proxies are synced via NetworkTransforms
            vrRig.SetActive(false);

            // Ceate IK target proxies
            var proxyRoot = new GameObject("IK Proxies").transform;
            proxyRoot.parent = transform;
            proxyRoot.localPosition = Vector3.zero;
            proxyRoot.localRotation = Quaternion.identity;

            headIKProxy = new GameObject("Head IK Proxy").transform;
            headIKProxy.position = ik.references.head.position;
            headIKProxy.rotation = ik.references.head.rotation;

            leftHandIKProxy = new GameObject("Left Hand IK Proxy").transform;
            leftHandIKProxy.position = ik.references.leftHand.position;
            leftHandIKProxy.rotation = ik.references.leftHand.rotation;

            rightHandIKProxy = new GameObject("Right Hand IK Proxy").transform;
            rightHandIKProxy.position = ik.references.rightHand.position;
            rightHandIKProxy.rotation = ik.references.rightHand.rotation;

            // Assign proxies as IK targets for the remote instance
            ik.solver.spine.headTarget = headIKProxy;
            ik.solver.leftArm.target = leftHandIKProxy;
            ik.solver.rightArm.target = rightHandIKProxy;

            // Just for debugging
            if (remoteMaterialOverride != null)
            {
                ik.references.root.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = remoteMaterialOverride;
            }
        }

        private void UpdateRemote()
        {
            // Apply synced position/rotations to proxies
            if (ik.solver.locomotion.weight <= 0) rootNetworkT.ApplyRemoteInterpolated(ik.references.root, proxyInterpolationSpeed, proxyMaxErrorSqrMag); // Only sync root when using animated locomotion. Procedural locomotion follows head IK proxy anyway
            headNetworkT.ApplyRemoteInterpolated(headIKProxy, proxyInterpolationSpeed, proxyMaxErrorSqrMag);
            leftHandNetworkT.ApplyRemoteInterpolated(leftHandIKProxy, proxyInterpolationSpeed, proxyMaxErrorSqrMag);
            rightHandNetworkT.ApplyRemoteInterpolated(rightHandIKProxy, proxyInterpolationSpeed, proxyMaxErrorSqrMag);
        }

        #endregion Remote
    }
}
