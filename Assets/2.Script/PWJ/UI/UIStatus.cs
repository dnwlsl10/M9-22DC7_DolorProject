using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : UIBase 
{
    [Header("UIGuidedMissile")]
    public Image fillamount_progress;
    private Coroutine coroutineHolder;

    public override void OnComplete() => base.OnComplete();
    public override void OnDefult() => base.OnDefult();

    public override void EventValue(float hp, float maxHP)
    {
     
        value = hp / maxHP;
        fillamount_progress.fillAmount = value;
        textProgress.text = hp.ToString();

        if (hp == 100){
            OnDefult();
            return;
        }
        
        if(coroutineHolder !=null){
            StopCoroutine(coroutineHolder);
            coroutineHolder =null;
        }

        if (hp >= 0)
            coroutineHolder = StartCoroutine(OnDelay());
    }

   IEnumerator OnDelay()
    {
        OnComplete();
        yield return new WaitForSeconds(1f);
        OnDefult();
    }

}
