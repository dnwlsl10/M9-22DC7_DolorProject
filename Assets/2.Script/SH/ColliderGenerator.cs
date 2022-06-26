using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderGenerator : MonoBehaviour, IInitialize
{
    [ContextMenu("Attach Scripts")]
    void AddScripts()
    {
        RootMotion.FinalIK.VRIK vrik = GetComponent<RootMotion.FinalIK.VRIK>();
        if (vrik == null)
        {
            Debug.LogError("No VRIK Found");
            return;
        }

        AttachScript(vrik.references.spine.gameObject);

        AttachScript(vrik.references.leftThigh.gameObject);
        AttachScript(vrik.references.rightThigh.gameObject);

        AttachScript(vrik.references.leftCalf.gameObject);
        AttachScript(vrik.references.rightCalf.gameObject);

        AttachScript(Utility.FindChildContainsName(vrik.references.leftUpperArm, new string[] {"Col", "col"}).gameObject);
        AttachScript(Utility.FindChildContainsName(vrik.references.rightUpperArm, new string[] {"Col", "col"}).gameObject);

        AttachScript(vrik.references.leftForearm.gameObject);
        AttachScript(vrik.references.rightForearm.gameObject);
    }
    public List<Object> attchScriptList;
    public void Reset() {
        RootMotion.FinalIK.VRIK vrik = GetComponent<RootMotion.FinalIK.VRIK>();
        if (vrik == null)
        {
            Debug.LogError("No VRIK Found");
            return;
        }
        AttachCollider(vrik.references.spine, new Vector3(5.76339912e-18f,0.00717358943f,-6.1715677e-10f), 0.008f, 0.0291208f);

        AttachCollider(vrik.references.leftThigh, new Vector3(-2.397299e-05f, 0.008548272f, -0.001282697f), 0.004660634f, 0.0229034f);
        AttachCollider(vrik.references.rightThigh, new Vector3(-2.397299e-05f, 0.008548272f, -0.001282697f), 0.004660634f, 0.0229034f);

        AttachCollider(vrik.references.leftCalf, new Vector3(7.57556691e-12f,0.0163353719f,4.82889978e-11f), 0.005f, 0.03552755f);
        AttachCollider(vrik.references.rightCalf, new Vector3(7.57556691e-12f,0.0163353719f,4.82889978e-11f), 0.005f, 0.03552755f);

        AttachCollider(Utility.FindChildContainsName(vrik.references.leftUpperArm, new string[] {"Col", "col"}), Vector3.right * -0.001f, 0.003f, 0.02f);
        AttachCollider(Utility.FindChildContainsName(vrik.references.rightUpperArm, new string[] {"Col", "col"}), Vector3.right * 0.001f, 0.003f, 0.02f);

        AttachCollider(vrik.references.leftForearm, new Vector3(0.000537198328f,0.00722201215f,-0.000389008783f), 0.004073791f, 0.01676754f);
        AttachCollider(vrik.references.rightForearm, new Vector3(0.000537198328f,0.00722201215f,-0.000389008783f), 0.004073791f, 0.01676754f);
    }

    void AttachCollider(Transform target, Vector3 xyzOffset, float radius)
    {
        // foreach(Collider prevCol in target.GetComponents<Collider>())
        if (target.TryGetComponent<Collider>(out Collider prevCol))
            return;

        SphereCollider col = target.gameObject.AddComponent<SphereCollider>();
        col.center = xyzOffset;
        col.radius = radius;

        target.gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
    }

    void AttachCollider(Transform target, Vector3 xyzOffset, float radius, float height)
    {
        // foreach(Collider prevCol in target.GetComponents<Collider>())
        if (target.TryGetComponent<Collider>(out Collider prevCol))
            return;

        CapsuleCollider col = target.gameObject.AddComponent<CapsuleCollider>();
        col.center = xyzOffset;
        col.radius = radius;
        col.height = height;
        col.direction = 1;

        target.gameObject.layer = LayerMask.NameToLayer("LocalPlayer");
    }

    void AttachScript(GameObject target)
    {
        foreach (Object script in attchScriptList)
        {
            print(script.GetType());
            System.Type type = null;
            if (script.GetType() == typeof(UnityEditor.MonoScript))
            {
                type = (script as UnityEditor.MonoScript).GetClass();
            }
            else
            {
                type = script.GetType();
            }

            Component _component = null;
            if (target.TryGetComponent(type, out _component) == false)
            {
                _component = target.AddComponent(type);
            }

            if (_component is Photon.Pun.PhotonView)
            {
                (_component as Photon.Pun.PhotonView).Synchronization = Photon.Pun.ViewSynchronization.Off;
            }
        }
    }
}
