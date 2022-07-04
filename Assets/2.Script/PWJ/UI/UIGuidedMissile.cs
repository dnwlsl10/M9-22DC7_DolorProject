using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIGuidedMissile : UIBase
{
    [Header("UIGuidedMissile")]
    public Image fillamount_progress;
    private bool isFire = false;

    public override void EventValue(float current, float max){
        
        value = current / max;
        fillamount_progress.fillAmount = value;
        textProgress.text = ((int)(value * 100)).ToString();

        if(isFire){
            disableUIKeys.gameObject.SetActive(true);
            enableUIKeys.gameObject.SetActive(false);
            if (fillamount_progress.fillAmount != 0) return;
            else isFire = false;
            OnOFF(available, reload);
        }
       
        if(!textProgress.gameObject.activeSelf) textProgress.gameObject.SetActive(true);
        
        if(current == max){
            Set();
            isFire = true;
            textProgress.gameObject.SetActive(false);
        }
    }
}
