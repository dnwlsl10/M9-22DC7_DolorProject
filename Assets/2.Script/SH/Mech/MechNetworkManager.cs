using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using Photon.Pun;
public class MechNetworkManager : MonoBehaviour
{
    void Reset()
    {
        FindAll();
    }
    [ContextMenu("Find All")]
    void FindAll()
    {
        Transform root = GetComponent<VRIK>().references.pelvis.parent;
        Transform meshRoot = root.Find("mesh");
        if (meshRoot == null)
            meshRoot = root.Find("Mesh");
        localDisableMesh = new List<Renderer>();
        for (int i = 0; i < meshRoot.childCount; i++)
        {
            Transform child = meshRoot.GetChild(i);
            if (child.name.Equals("Arm")) continue;

            Renderer[] tmp = child.GetComponentsInChildren<Renderer>();
            localDisableMesh.AddRange(tmp);
        }

        LayerToChangeRemote = new List<GameObject>();
        Collider[] cols = root.GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
            LayerToChangeRemote.Add(col.gameObject);
    }

    [ContextMenu("Toggle Mesh")]
    void ToggleMesh()
    {
        foreach (var renderer in localDisableMesh)
            renderer.enabled = !renderer.enabled;
    }
    public List<Renderer> localDisableMesh;
    public List<GameObject> LayerToChangeRemote;
    IKSolverVR.Arm rightArmIK;
    IKSolverVR.Arm leftArmIK;
    PhotonView pv;

    IEnumerator leftIKCoroutine;
    IEnumerator rightIKCoroutine;

    private void Start() 
    {
        this.pv = this.GetComponent<PhotonView>();
        if (pv.Mine)
            SetLocal();
        else
            SetRemote();
    }

    private void DisableMesh(List<Renderer> meshList)
    {
        foreach (var mesh in meshList)
            mesh.enabled = false;
        meshList.Clear();
        meshList = null;
    }

    void SetLocal()
    {
        DisableMesh(localDisableMesh);
    }
    void SetRemote()
    {
        ChangeRemoteLayer();
        GetComponent<Animator>().applyRootMotion = false;
    }
    void ChangeRemoteLayer()
    {
        foreach (var obj in LayerToChangeRemote)
            obj.layer = LayerMask.NameToLayer("RemotePlayer");
        LayerToChangeRemote.Clear();
        LayerToChangeRemote = null;
    }
}
