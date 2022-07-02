using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuidedMissile : UIBase  , IUIButton
{
    [Header("UIButoon")]
    public Image onClickFirstHelpkey;
    public Image offClickFirstHelpkey;
    public Image onClickSecondHelpkey;
    public Image offClickSecondHelpkey;

    public GameObject enableHelpUI; //노멀 키
    public GameObject disableHelpUI;

    [Header("UIGuidedMissile")]
    public Image fillamount_progress;
    private bool isFire;


    public override void OnComplete(){
        isFire = true;
        textProgress.gameObject.SetActive(false);
        base.OnComplete();
    }

    public override void EventValue(float current, float max){
        
        value = current / max;
        fillamount_progress.fillAmount = value;

        if(isFire && fillamount_progress.fillAmount !=0) return;
        isFire = false;

        if (current >= max) StartUI();
        else StopUI();
    }
    public override void StartUI()
    {
        OnComplete();
        OnButton();
    }
    public override void StopUI()
    {
        OnDefult();
        OffButton();
    }

    public override void OnDefult(){

        textProgress.gameObject.SetActive(true);
        textProgress.text = (value * 100f).ToString();
        base.OnDefult();
    }

    public void OnButton() //활성화
    {
        enableHelpUI.gameObject.SetActive(true);
        disableHelpUI.gameObject.SetActive(false);
    }

    public void OffButton() //비활성화 
    {
        enableHelpUI.gameObject.SetActive(false);
        disableHelpUI.gameObject.SetActive(true);
    }

    public void OnFirstButton()
    {
        onClickFirstHelpkey.gameObject.SetActive(true);
        offClickFirstHelpkey.gameObject.SetActive(false);
    }

    public void OffFirstButton()
    {
        onClickFirstHelpkey.gameObject.SetActive(false);
        offClickFirstHelpkey.gameObject.SetActive(true);
    }

    public void OnSecondButton()
    {
        onClickSecondHelpkey.gameObject.SetActive(true);
        offClickSecondHelpkey.gameObject.SetActive(false);
    }

    public void OffSecondButton()
    {
        onClickSecondHelpkey.gameObject.SetActive(false);
        offClickSecondHelpkey.gameObject.SetActive(true);
    }

}
