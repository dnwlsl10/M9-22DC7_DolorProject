using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrb : UIBase
{
    [Header("Pivot")]
    public Transform originOrbA;
    public Transform originOrbB;
    public Transform originOrbC;
    [Header("OrbB")]
    public GameObject orbBAvailable;
    public GameObject orbBReload;
    [Header("OrbC")]
    public GameObject orbCAvailable;
    public GameObject orbCReload;
    public int orbType;
    Vector3 tmp;
    Vector3 tmpB;
    Vector3 tmpC;
    
    public void OnSelectedOrb(int num)
    {
        this.orbType = num;
    }

    public IEnumerator OnLerpUI(bool isSelected){
        tmp = originOrbA.position;
        tmpB = originOrbB.position;
        tmpC = originOrbC.position;
        while(!isSelected)
        {
            yield return null;
            originOrbA.position = Vector3.Lerp(originOrbA.position, tmpC , Time.deltaTime);
            originOrbC.position = Vector3.Lerp(originOrbC.position, tmpB, Time.deltaTime);
            originOrbB.position = Vector3.Lerp(originOrbB.position, tmp , Time.deltaTime);
        }
        originOrbA.position = tmpC;
        originOrbC.position = tmpB;
        originOrbB.position = tmp;

        tmp = originOrbA.position;
        tmpB = originOrbB.position;
        tmpC = originOrbC.position;
        isSelected = false;
    }


    //Start On
    public bool bCanUse;
    public override void EventValue(float coolval, float attackRate)
    {
        if(coolval == attackRate)
        {
            available.SetActive(true); //변경대상//

            enableUIKeys.SetActive(true);
            disableUIKeys.SetActive(false);
            textProgress.gameObject.SetActive(false);
            bCanUse = true;
        }
        else if(bCanUse || coolval < attackRate)
        {
            bCanUse = false;

            available.SetActive(false); //변경대상//

            enableUIKeys.SetActive(false);
            disableUIKeys.SetActive(true);
            textProgress.gameObject.SetActive(true);
            textProgress.text = coolval.ToString();
        }
    }
}
