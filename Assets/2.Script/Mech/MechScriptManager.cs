using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechScriptManager : MonoBehaviour
{
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
