using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrb : UIBase
{
    [Header("Pivot")]
    public Transform originOrbA;
    public Transform originOrbB;
    [Header("OrbB")]
    public GameObject orbBAvailable;
    public GameObject orbBReload;

    public int orbType;
    public bool isLock;

    public GameObject[] orbAvailable;
    public GameObject[] orbReload;

    Vector3 tmp;
    Vector3 tmpB;

    public void Awake()
    {
        orbAvailable = new GameObject[2] { available, orbBAvailable, };
        orbReload = new GameObject[2] {reload , orbBReload};
    }
    public IEnumerator OnLerpUI(int typeNum,System.Action OnComplete)
    {

        this.orbType = typeNum;

        if(!bRealoding){
            OnUse();
            OffUse();
        }

        tmp = originOrbA.localPosition;
        tmpB = originOrbB.localPosition;

        while(Vector3.Distance(originOrbA.localPosition, tmpB) > 0.1f){
            Debug.Log("test");
            yield return new WaitForEndOfFrame();
            originOrbA.localPosition = Vector3.Lerp(originOrbA.localPosition, tmpB, Time.deltaTime);
            originOrbB.localPosition = Vector3.Lerp(originOrbB.localPosition, tmp, Time.deltaTime);
        }

        originOrbA.localPosition = tmpB;
        originOrbB.localPosition = tmp;

        OnComplete();
    }

    void OnUse()
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
    void OffUse()
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
    float maxVal;
    public override void EventValue(float coolval, float attackRate)
    {   maxVal = attackRate;
        if (isLock) return;
        //쿨타임이 attackRate 와 같을때 스킬이 사용 가능하다.

        if(coolval == 0) OffSecondButton();
        if(coolval == attackRate) OnComplete();
        if(coolval < attackRate) OnValueUp(coolval);
    }

    public void OnComplete(){
        bRealoding = false;
        orbAvailable[this.orbType].SetActive(true);
        orbReload[this.orbType].SetActive(false);
        enableUIKeys.SetActive(true);
        disableUIKeys.SetActive(false);
        textProgress.gameObject.SetActive(false);
    }
    bool bRealoding;
    public override void OffSecondButton()
    {
        bRealoding = true;
        base.OffSecondButton();
        enableUIKeys.SetActive(false);
        disableUIKeys.SetActive(true);
        orbAvailable[this.orbType].gameObject.SetActive(false);
        orbReload[this.orbType].gameObject.SetActive(true);
        textProgress.gameObject.SetActive(true);
    }

    public override void OnValueUp(float val)
    {
        if(val <= 0) textProgress.text = maxVal.ToString();
        else textProgress.text = val.ToString();
    }

    private void OnEnable() {
        WeaponSystem.instance.AddStartStopCallbackTarget(this, (int)WeaponName.Orb);
    }
    private void OnDisable() {
        WeaponSystem.instance.RemoveStartStopCallbackTarget(this, (int)WeaponName.Orb);
    }
}
