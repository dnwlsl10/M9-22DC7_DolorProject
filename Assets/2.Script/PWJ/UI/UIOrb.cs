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

    public GameObject[] orbAvailable;
    public GameObject[] orbReload;

    Vector3 tmp;
    Vector3 tmpB;
    Vector3 tmpC;
    public System.Action<int> OnSelectedOrbType;

    public void Awake()
    {
        orbAvailable = new GameObject[3] { available, orbBAvailable, orbCAvailable};
        orbReload = new GameObject[3] {reload , orbBReload, orbCReload };
    }

    public void OnSelectedOrb(int num)
    {
        this.orbType = num;
        OnSelectedOrbType(this.orbType);
    }

    public void OnLerpUI(int etype)
    {
        orbType = etype;
        tmp = originOrbA.localPosition;
        tmpB = originOrbB.localPosition;
        tmpC = originOrbC.localPosition;

        originOrbA.localPosition = Vector3.Lerp(originOrbA.localPosition, tmpC , Time.deltaTime);
        originOrbC.localPosition = Vector3.Lerp(originOrbC.localPosition, tmpB, Time.deltaTime);
        originOrbB.localPosition = Vector3.Lerp(originOrbB.localPosition, tmp , Time.deltaTime);
   
        originOrbA.localPosition = tmpC;
        originOrbC.localPosition = tmpB;
        originOrbB.localPosition = tmp;

        tmp = originOrbA.localPosition;
        tmpB = originOrbB.localPosition;
        tmpC = originOrbC.localPosition;

        if(isUseing){
            ResultOn();
            ResultOFF();
        }
    }

    void ResultOn()
    {
        for (int i = 0; i < orbAvailable.Length; i++)
        {
            if (i == orbType)
            {
                orbAvailable[i].gameObject.SetActive(true);
            }
            else
            {
                orbAvailable[i].gameObject.SetActive(false);
            }
        }
    }
    void ResultOFF()
    {
        for (int i = 0; i < orbReload.Length; i++)
        {
            if (i == orbType)
            {
                orbReload[i].gameObject.SetActive(false);
            }
            else
            {
                orbReload[i].gameObject.SetActive(true);
            }
        }
    }

    //Start On
    bool isUseing;
    public override void EventValue(float coolval, float attackRate)
    {
        if(coolval == 0) {
            enableUIKeys.SetActive(false);
            disableUIKeys.SetActive(true);
            isUseing = false;
        }

        if(coolval == attackRate)
        {
            orbAvailable[this.orbType].SetActive(true);
            orbReload[this.orbType].SetActive(false);

            enableUIKeys.SetActive(true);
            disableUIKeys.SetActive(false);

            textProgress.gameObject.SetActive(false);
        }
        else if(coolval < attackRate)
        {
            isUseing =true;
            
            if(isUseing){
                orbReload[this.orbType].SetActive(true);
                orbAvailable[this.orbType].SetActive(false);
                enableUIKeys.SetActive(false);
                disableUIKeys.SetActive(true);
            }
            textProgress.gameObject.SetActive(true);
            textProgress.text = coolval.ToString();
        }
    }
}
