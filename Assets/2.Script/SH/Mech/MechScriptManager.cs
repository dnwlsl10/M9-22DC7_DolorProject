using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MechScriptManager : MonoBehaviour
{
    public Behaviour[] scriptsForOnlyLocal;
    PhotonView pv;

    private void Awake() 
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine == false)
            foreach (var script in scriptsForOnlyLocal)
                Destroy(script);
    }
    public void EnableScripts(ref Behaviour[] components)
    {
        foreach (var component in components)
        {
            if (component!=null)
                component.enabled = true;
        }
    }
    public void DisableScripts(ref Behaviour[] components)
    {
        foreach (var component in components)
        {
            if (component!=null)
                component.enabled = false;
        }
    }
}
