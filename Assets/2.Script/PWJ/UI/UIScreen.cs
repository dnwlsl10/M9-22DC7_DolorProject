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
        uIOrb.textProgress.gameObject.SetActive(false);
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
        status.OnValueChange -= uIStatus.EventValue;
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.M)){
            status.TakeDamage(5f, Vector3.zero);
        }
    }
}
