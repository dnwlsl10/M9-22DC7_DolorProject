using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : UIBase 
{
    [Header("UIGuidedMissile")]
    public Image fillamount_progress;
    private Coroutine coroutineHolder;

    public override void EventValue(float hp, float maxHP)
    {
        value = hp / maxHP;
        fillamount_progress.fillAmount = value;
        textProgress.text = hp.ToString();

        if (hp == 0){
            available.gameObject.SetActive(false);
            available.gameObject.SetActive(true);
        }
        else{
            if (coroutineHolder != null)
            {
                StopCoroutine(coroutineHolder);
                coroutineHolder = null;
            }

            if (hp > 0)
                coroutineHolder = StartCoroutine(OnDelay());
        }
        
    }

   IEnumerator OnDelay()
    {
        Set();
        yield return new WaitForSeconds(1f);
        Set();
    }

}
