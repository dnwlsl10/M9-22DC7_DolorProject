using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockPit : MonoBehaviour
{
    public UIEarth uIEarth;
    public UIScreen uIScreen;

    public Light particePointLight;
    public Light playDefultLight;

    private void OnEnable() {
        this.uIEarth.gameObject.SetActive(false);
        this.uIScreen.gameObject.SetActive(true);    
    }

    public void EnterParticleMode(){
        particePointLight.gameObject.SetActive(true);
    }
    public void ExitParticleMode()
    {
        particePointLight.gameObject.SetActive(false);
    }

    public void FindOtherPlayer(){
        uIEarth.OnRaycast();
        playDefultLight.gameObject.SetActive(true);
    }

    public void EnterQuickMatchMode(){
        this.uIEarth.Init();
  
    }
    public void ExitQuickMatchMode()
    {
        this.uIEarth.Exit();

    }
    public void UnLockMode() => uIScreen.UnLockMode(); 

    public void LockMode() => uIScreen.LockMode();
}
