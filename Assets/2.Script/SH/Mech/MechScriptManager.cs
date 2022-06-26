using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MechScriptManager : MonoBehaviour
{
    void Reset()
    {
        Init();
    }
    [ContextMenu("Initialize")]
    void Init()
    {
        componentsForOnlyLocal = new List<Component>();
        componentsForOnlyLocal.Add(transform.root.GetComponent<Rigidbody>());
        componentsForOnlyLocal.AddRange(transform.root.GetComponentsInChildren<HandIK>());
        componentsForOnlyLocal.AddRange(transform.root.GetComponentsInChildren<CrossHair>());
        componentsForOnlyLocal.AddRange(transform.root.GetComponentsInChildren<WeaponSystem>());
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
    public void EnableScripts(ref List<Behaviour> components)
    {
        foreach (var component in components)
        {
            if (component!=null)
                component.enabled = true;
        }
    }
    public void DisableScripts(ref List<Behaviour> components)
    {
        foreach (var component in components)
        {
            if (component!=null)
                component.enabled = false;
        }
    }
}
