using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
public class UIShield : UIBase
{
    [Header("UIShield")]
    public Image fillamount_progress;
    private bool bOver;
    public bool bLock;
    public override void EventValue(float current, float max)
    {
        if (bLock) return;
        value = current / max;
        fillamount_progress.fillAmount = value;

        if(bOver)
        {
             if(current <= 30) return;
             else
                bOver = false;
              Set();
        }
        
        if(current == 0)
        {
            bOver = true;
            Set();
        }
    }

    private void OnEnable() {
        WeaponSystem.instance.AddStartStopCallbackTarget(this, (int)WeaponName.Shield);
    }
    private void OnDisable() {
        WeaponSystem.instance.RemoveStartStopCallbackTarget(this, (int)WeaponName.Shield);
    }
}
