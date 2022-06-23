using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using RootMotion.FinalIK;
using RootMotion.Demos;

public class Initializer : MonoBehaviour
{
    [ContextMenu("AttachScript")]
    void Init()
    {
        Transform root = transform.root;

        CheckComponentExist(root, typeof(Status));
        CheckComponentExist(root, typeof(Animator));
        CheckComponentExist(root, typeof(PhotonView));
        CheckComponentExist(root, typeof(PhotonAnimatorView));
        CheckComponentExist(root, typeof(Rigidbody));
        CheckComponentExist(root, typeof(CapsuleCollider));
        CheckComponentExist(root, typeof(VRIK));
        CheckComponentExist(root, typeof(VRIK_PUN_Player));
        CheckComponentExist(root, typeof(MechScriptManager));
        CheckComponentExist(root, typeof(MechMovementController));
        CheckComponentExist(root, typeof(MechLand));
        CheckComponentExist(root, typeof(MechNetworkManager));
        CheckComponentExist(root, typeof(IKWeight));

        Transform child = root.Find("IKTarget");
        Transform leftTarget = null;
        Transform rightTarget = null;
        if (child == null)
        {
            child = new GameObject("IKTarget").transform;
            child.parent = root;

            leftTarget = new GameObject("LeftHandTarget").transform;
            leftTarget.parent = child;

            rightTarget = new GameObject("RightHandTarget").transform;
            rightTarget.parent = child;
        }
        else
        {
            leftTarget = child.Find("LeftHandTarget");
            rightTarget = child.Find("RightHandTarget");
        }

        CheckComponentExist(leftTarget, typeof(Rigidbody));
        CheckComponentExist(leftTarget, typeof(HandIK));
        CheckComponentExist(leftTarget, typeof(SphereCollider));

        CheckComponentExist(rightTarget, typeof(Rigidbody));
        CheckComponentExist(rightTarget, typeof(HandIK));
        CheckComponentExist(rightTarget, typeof(SphereCollider));
        
        child = root.Find("WeaponScript");
        Transform weapon = null;
        if (child == null)
        {
            child = new GameObject("WeaponScript").transform;
            child.parent = root;

            weapon = new GameObject("BasicWeapon").transform;
            weapon.parent = child;
        }
        else
            weapon = child.Find("BasicWeapon");
        CheckComponentExist(child, typeof(GrabEvent));

        CheckComponentExist(weapon, typeof(PhotonView));
        CheckComponentExist(weapon, typeof(BasicWeapon));
        CheckComponentExist(weapon, typeof(CrossHair));
    }

    Component CheckComponentExist(Transform target, System.Type type)
    {
        if (target.TryGetComponent(type, out Component _component) == false)
        {
            _component = target.gameObject.AddComponent(type);
        }

        return _component;
    }
}
