using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MechScriptManager : MonoBehaviour
{
    [ContextMenu("Initialize")]
    void Reset()
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
#endif
    }
    
    public List<Component> componentsForOnlyLocal;
    PhotonView pv;

    private void Awake() 
    {
        pv = GetComponent<PhotonView>();

        if (pv.Mine == false)
            foreach (var component in componentsForOnlyLocal)
                Destroy(component);

        componentsForOnlyLocal = null;
    }
}
