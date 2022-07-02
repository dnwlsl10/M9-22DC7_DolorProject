using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Reflection;

public class UIScreen : MonoBehaviour
{
    public UIBasicWeapon uIBasicWeapon;
    public UIGuidedMissile uIGuidedMissile; 
    public UIStatus uIStatus;
    public UIShield uIShield;
    [SerializeField] private Status status;
    [SerializeField] private BasicWeapon bw;
    [SerializeField] private SkillShield sw;
    [SerializeField] private GuidedMissile gm;

    public void OnEnable(){

        bw.OnValueChange += uIBasicWeapon.EventValue;
        bw.OnPress = () => {uIBasicWeapon.OnSecondButton();};
        bw.OnCancle =() => {uIBasicWeapon.OffSecondButton();};

        gm.OnValueChange += uIGuidedMissile.EventValue;
        gm.OnPress = () => {uIGuidedMissile.OnSecondButton();};
        gm.OnCancle = () => {uIGuidedMissile.OffSecondButton();};

        sw.OnValueChange += uIShield.EventValue;
        sw.OnPress = () =>{uIShield.OnSecondButton();};
        sw.OnCancle = () =>{uIShield.OffSecondButton();};

        status.OnValueChange += uIStatus.EventValue;
    }

    private void OnDisable() {
        bw.OnValueChange -= uIBasicWeapon.EventValue;
        gm.OnValueChange -= uIGuidedMissile.EventValue;
        sw.OnValueChange -= uIShield.EventValue;
        status.OnValueChange -= uIStatus.EventValue;
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.M)){
            status.TakeDamage(5f);
        }
    }
}
