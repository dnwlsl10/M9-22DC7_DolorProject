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
        tmp = originOrbA.localPosition;
        tmpB = originOrbB.localPosition;
        tmpC = originOrbC.localPosition;
    }

    public void OnSelectedOrb(int num)
    {
        this.orbType = num;
        OnSelectedOrbType(this.orbType);
    }

    public IEnumerator OnLerpUI(bool isSelected){

        while(isSelected)
        {
            yield return null;
            originOrbA.localPosition = Vector3.Lerp(originOrbA.localPosition, tmpC , Time.deltaTime);
            originOrbC.localPosition = Vector3.Lerp(originOrbC.localPosition, tmpB, Time.deltaTime);
            originOrbB.localPosition = Vector3.Lerp(originOrbB.localPosition, tmp , Time.deltaTime);
        }
        originOrbA.localPosition = tmpC;
        originOrbC.localPosition = tmpB;
        originOrbB.localPosition = tmp;

        tmp = originOrbA.localPosition;
        tmpB = originOrbB.localPosition;
        tmpC = originOrbC.localPosition;
    }

    void OnTest()
    {
        for (int i = 0; i < orbAvailable.Length; i++)
        {
            if (i == orbType)
            {
                orbAvailable[this.orbType].SetActive(true); //변경대상//
            }
            else
            {
                orbAvailable[i].SetActive(false);
            }
        }
    }


    //Start On
    public bool bCanUse;
    public override void EventValue(float coolval, float attackRate)
    {
        if(coolval == attackRate)
        {
            for (int i = 0; i < orbAvailable.Length; i++)
            {
                if (i == orbType)
                {
                    orbAvailable[this.orbType].SetActive(true); //변경대상//
                }
            }
            enableUIKeys.SetActive(true);
            disableUIKeys.SetActive(false);
            textProgress.gameObject.SetActive(false);
            bCanUse = true;
        }
        else if(bCanUse || coolval < attackRate)
        {
            bCanUse = false;

            for (int i = 0; i < orbReload.Length; i++)
            {
                orbAvailable[this.orbType].SetActive(false);
                orbReload[this.orbType].SetActive(true); //변경대상//
            }

            enableUIKeys.SetActive(false);
            disableUIKeys.SetActive(true);
            textProgress.gameObject.SetActive(true);
            textProgress.text = coolval.ToString();
        }
    }
}
