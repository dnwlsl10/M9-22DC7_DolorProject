using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmControllerLights : MonoBehaviour
{
    public Light[] lights;
    
    public void ChangeMars()
    {
        for(int i = 0; i < lights.Length ; i++)
        {
            lights[i].color = new Color(0.5330188f, 0.7754576f, 1f, 1f);
        }
    }

    public void ChangeClavis()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].color = new Color(1,0,0,1);
        }
    }
}
