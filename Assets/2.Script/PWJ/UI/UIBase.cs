using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIBase : MonoBehaviour
{
    [Header("UIBase")]
    public Image complete;
    public Image reloading;
    public Text textProgress;
    public float value;

    public virtual void EventValue(float current, float max){}
    public virtual void StartUI(){}
    public virtual void StopUI(){}
    public virtual void OnComplete()
    {
        reloading.gameObject.SetActive(false);
        complete.gameObject.SetActive(true);
    }

    public virtual void OnDefult()
    {
        reloading.gameObject.SetActive(true);
        complete.gameObject.SetActive(false);
    }
}
