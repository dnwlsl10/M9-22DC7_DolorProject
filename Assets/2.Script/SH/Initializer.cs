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
        StartCoroutine(A());
    }

    IEnumerator A()
    {
        Transform root = transform.root;
        Transform child = root.Find("IKTarget");
        if (child == null)
        {
            (UnityEditor.PrefabUtility.InstantiatePrefab(Utility.Load<GameObject>("Assets/5.Prefabs/SHPrefab/IKTarget.prefab")) as GameObject).transform.parent = root;
        }

        yield return new WaitForSecondsRealtime(0.01f);
        child = root.Find("WeaponScript");
        if (child == null)
        {
            (UnityEditor.PrefabUtility.InstantiatePrefab(Utility.Load<GameObject>("Assets/5.Prefabs/SHPrefab/WeaponScript.prefab")) as GameObject).transform.parent = root;
        }
        yield return new WaitForSecondsRealtime(0.01f);

        CheckComponentExist(root, typeof(ColliderGenerator));
        yield return new WaitForSecondsRealtime(0.01f);
        
        CheckComponentExist(root, typeof(Status));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(Animator));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(PhotonView));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(PhotonAnimatorView));
        yield return new WaitForSecondsRealtime(0.01f);
        Rigidbody rb = CheckComponentExist(root, typeof(Rigidbody)) as Rigidbody;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 378;
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(CapsuleCollider));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(VRIK));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(VRIK_PUN_Player));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(MechScriptManager));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(MechMovementController));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(MechLand));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(MechNetworkManager));
        yield return new WaitForSecondsRealtime(0.01f);
        CheckComponentExist(root, typeof(IKWeight));
        yield return new WaitForSecondsRealtime(0.01f);

        foreach(var init in transform.root.GetComponentsInChildren<IInitialize>())
        {
            init.Reset();
            yield return new WaitForSecondsRealtime(0.1f);
        }
        foreach(var init in transform.root.GetComponentsInChildren<IInitialize>())
        {
            init.Reset();
            yield return new WaitForSecondsRealtime(0.1f);
        }
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
