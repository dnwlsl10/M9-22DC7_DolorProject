using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.SceneManagement;

public class UIScreen : MonoBehaviour
{
    public UIBasicWeapon uIBasicWeapon;
    public UIGuidedMissile uIGuidedMissile; 
    public UIStatus uIStatus;
    public UIShield uIShield;

    public UIOrb uIOrb;
    [SerializeField] private Status status;
    [SerializeField] private BasicWeapon bw;
    [SerializeField] private SkillShield sw;
    [SerializeField] private GuidedMissile gm;
    [SerializeField] private OrbFire of;

    private bool bLock;
    public void OnEnable(){
        bw.OnValueChange += uIBasicWeapon.EventValue;
        WeaponSystem.instance.onStartAction[(int)WeaponName.Basic] += uIBasicWeapon.OnSecondButton;
        WeaponSystem.instance.onStopAction[(int)WeaponName.Basic] += uIBasicWeapon.OffSecondButton;

        gm.OnValueChange += uIGuidedMissile.EventValue;
        WeaponSystem.instance.onStartAction[(int)WeaponName.Missile] += uIGuidedMissile.OnSecondButton;
        WeaponSystem.instance.onStopAction[(int)WeaponName.Missile] += uIGuidedMissile.OffSecondButton;

        sw.OnValueChange += uIShield.EventValue;
        WeaponSystem.instance.onStartAction[(int)WeaponName.Shield] += uIShield.OnSecondButton;
        WeaponSystem.instance.onStopAction[(int)WeaponName.Shield] += uIShield.OffSecondButton;

        of.OnValueChange += uIOrb.EventValue;
        WeaponSystem.instance.onStartAction[(int)WeaponName.Orb] += uIOrb.OnSecondButton;
        WeaponSystem.instance.onStopAction[(int)WeaponName.Orb] += uIOrb.OffSecondButton;

        status.OnValueChange += uIStatus.EventValue;
    }
  
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Connect") LockMode();
    }

    public void LockMode()
    {
        Debug.Log("Lock");
        bw.CurrentAmmo = 0;

        gm.CurrentAmmo = 0;

        sw.CurrentAmmo = 0;
        sw.weaponSetting.bLock = true;

        of.Cooldown = 0;
        uIOrb.textProgress.text = of.weaponSetting.maxAmmo.ToString();
        of.weaponSetting.bLock = true;
    }

    public void UnLockMode()
    {
        Debug.Log("UnLock");
        bw.weaponSetting.bLock = false;
        bw.CurrentAmmo = bw.weaponSetting.maxAmmo;

        gm.weaponSetting.bLock = false;
        gm.StartReload();

        sw.weaponSetting.bLock = false;
        of.Cooldown = of.weaponSetting.attackRate;
        of.weaponSetting.bLock = false;
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
