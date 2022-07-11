using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public interface ILock{
    public void OnLock();
    public void UnLock();
}
public class UIBase : MonoBehaviour  , ILock, IWeaponEvent
{

    [Header("UIBase")]
    public GameObject available;
    public GameObject reload;
    public Text textProgress;
    public float value;

    [Header("UIKey")]
    public GameObject onbtnF;
    public GameObject offbtnF;
    public GameObject onbtnS;
    public GameObject offbtnS;

    [Header("UIKeyParentActive")]
    public GameObject enableUIKeys; //노멀 키
    public GameObject disableUIKeys;


    public virtual void EventValue(float current, float max){}
    public virtual void OnValueUp(float val){}
    public virtual void OnValeDown(float val){}

    public void Set()
    {
        OnOFF(available, reload);
        OnOFF(enableUIKeys, disableUIKeys);
    }

    public void OnOFF(GameObject enable, GameObject disable)
    {
        bool bActive = enable.activeSelf ? true : false;

        enable.gameObject.SetActive(!bActive);
        disable.gameObject.SetActive(bActive);
    }

    public void OnFirstButton()
    {
        onbtnF.gameObject.SetActive(true);
        offbtnF.gameObject.SetActive(false);
    }

    public void OFFFirstButton()
    {
        onbtnF.gameObject.SetActive(false);
        offbtnF.gameObject.SetActive(true);
    }

    public virtual void OnSecondButton()
    {
        onbtnS.gameObject.SetActive(true);
        offbtnS.gameObject.SetActive(false);
    }

    public virtual void OffSecondButton()
    {
        onbtnS.gameObject.SetActive(false);
        offbtnS.gameObject.SetActive(true);
    }

    public void OnLock()
    {
        enableUIKeys.gameObject.SetActive(false);
        disableUIKeys.gameObject.SetActive(true);
    }

    public void UnLock()
    {
        enableUIKeys.gameObject.SetActive(true);
        disableUIKeys.gameObject.SetActive(false);
    }
}
