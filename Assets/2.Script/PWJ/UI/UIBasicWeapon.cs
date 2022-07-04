using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBasicWeapon : UIBase 
{
    bool isReloading;
    public override void EventValue(float current, float max)
    {
        value = current;
        textProgress.text = value.ToString();

        if (isReloading)
        {
            if (value == 0) return;
            else{
                isReloading = false;
                Set();
            } 
        }

        if (value == 0)
        {
            Set();
            isReloading = true;
        }
    }
}
