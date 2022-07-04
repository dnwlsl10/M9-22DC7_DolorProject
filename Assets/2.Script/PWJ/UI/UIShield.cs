using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
public class UIShield : UIBase
{
    [Header("UIShield")]
    public Image fillamount_progress;
    private bool isOver;

    public override void EventValue(float current, float max)
    {
        value = current / max;
        fillamount_progress.fillAmount = value;

        if(isOver)
        {
             if(current <= 30) return;
             else
              isOver = false;
              Set();
        }
        
        if(current > 0 && !isOver) Debug.Log("test");
        else if(current == 0){
            isOver = true;
            Set();
        }
    }
}
