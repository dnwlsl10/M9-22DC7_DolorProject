using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public Light defultLight;
    public Light damageLight;

    public static LightManager instance;
    public void Awake(){

        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }

    public void OnDefultLight(){
        defultLight.gameObject.SetActive(true);
    }

    public void OnDamageLight(){
        damageLight.gameObject.SetActive(true);
    }

    public void OFFDamageLight()
    {
        damageLight.gameObject.SetActive(false);
    }

}
