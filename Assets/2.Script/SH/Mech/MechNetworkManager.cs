using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using Photon.Pun;
public class MechNetworkManager : MonoBehaviourPun, IInitialize, IPunInstantiateMagicCallback
{
    [ContextMenu("Find All")]
    public void Reset()
    {
#if UNITY_EDITOR
        if (componentsForOnlyLocal.Count == 0)
        {
            componentsForOnlyLocal = new List<Component>();
            componentsForOnlyLocal.Add(transform.root.GetComponent<Rigidbody>());
            componentsForOnlyLocal.AddRange(transform.root.GetComponentsInChildren<HandIK>());
            componentsForOnlyLocal.AddRange(transform.root.GetComponentsInChildren<CrossHair>());
            componentsForOnlyLocal.AddRange(transform.root.GetComponentsInChildren<WeaponSystem>());
        }

        Transform root = GetComponent<VRIK>().references.pelvis.parent;
        Transform meshRoot = Utility.FindChildMatchName(root, new string[]{"Mesh", "mesh"});
        
        if (localDisableMesh.Count == 0)
        {
            localDisableMesh = new List<Renderer>();
            for (int i = 0; i < meshRoot.childCount; i++)
            {
                Transform child = meshRoot.GetChild(i);
                if (child.name.Equals("Arm")) continue;

                Renderer[] tmp = child.GetComponentsInChildren<Renderer>();
                localDisableMesh.AddRange(tmp);
            }
        }

        if (LayerToChangeRemote.Count == 0)
        {
            LayerToChangeRemote = new List<GameObject>();
            Collider[] cols = root.GetComponentsInChildren<Collider>();
            foreach (Collider col in cols)
                LayerToChangeRemote.Add(col.gameObject);
        }
#endif
    }

    [SerializeField] List<Renderer> localDisableMesh;
    [SerializeField] List<GameObject> LayerToChangeRemote;
    [SerializeField] List<Component> componentsForOnlyLocal;

    private void Awake() 
    {
        if (photonView.Mine)    SetLocal();
        else                    SetRemote();

        LayerToChangeRemote = null; localDisableMesh = null; componentsForOnlyLocal = null;
    }

    void SetLocal() { foreach (var mesh in localDisableMesh) mesh.enabled = false; }

    void SetRemote()
    {
        GetComponent<Animator>().applyRootMotion = false;

        int remoteLayer = LayerMask.NameToLayer("RemotePlayer");
        foreach (var obj in LayerToChangeRemote)
            obj.layer = remoteLayer;
            
        foreach (var component in componentsForOnlyLocal)
            if (component) Destroy(component);
        
        this.gameObject.tag = "Enemy";
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) { if (photonView.Mine) photonView.RPC("RegistSelf", RpcTarget.AllViaServer); }

    [PunRPC]
    void RegistSelf()
    {
        InGameManager.instance.RegisterMech(gameObject);
        this.enabled = false;
    }
}
