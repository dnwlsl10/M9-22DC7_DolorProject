using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class SkillShield : WeaponBase, IDamageable
{
    public void Reset()
    {
        weaponSetting.weaponName = WeaponName.Shield;
        weaponSetting.maxAmmo = 100;
        handSide = HandSide.Left;
        gaugeUpSpeed = 10;
        gaugeDownSpeed = 20;
    }
    public event Cur_MaxEvent OnValueChange;

    // public GameObject shieldCreatePos;
    // public GameObject shield;
    public float gaugeUpSpeed;
    public float gaugeDownSpeed;
    Animator anim;
    public System.Action OnPress;
    public System.Action OnCancle;
    

    // public UnityEngine.InputSystem.InputActionReference alpha1;

    public float CurrentAmmo
    {
        get { return weaponSetting.currentAmmo; }
        set
        {
            // curammo 1, value 1.1, maxammo 1
            float prevAmmo = weaponSetting.currentAmmo;

            weaponSetting.currentAmmo = Mathf.Clamp(value, 0f, weaponSetting.maxAmmo);

            if (prevAmmo != weaponSetting.currentAmmo)
            {
                OnValueChange?.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
                if (weaponSetting.currentAmmo == 0)
                {
                    StartReload();
                }
            }
        }
    }

    // private void OnEnable()
    // {
    //     if (photonView.Mine == false) return;

    //     alpha1.action.started += StartEvent;
    //     alpha1.action.canceled += StopEvent;
    // }
    // private void OnDisable()
    // {
    //     if (photonView.Mine == false) return;

    //     alpha1.action.started -= StartEvent;
    //     alpha1.action.canceled -= StopEvent;
    // }

    // void Start()
    // {
    //     anim = GetComponent<Animator>();
    // }

    private void Start(){
        anim = GetComponent<Animator>();
        anim.Play("ShieldOff", 0, 1);
    }

    // void StartEvent(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    // {
    //     StartWeaponAction();
    // }
    // void StopEvent(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    // {
    //     StopWeaponAction();
    // }

    public override void StartWeaponAction() //GetKeyDown
    {
        if (isReloading || bLock)
            return;
        photonView.CustomRPC(this, "animPlay", RpcTarget.All, true);
        StartCoroutine("OnShieldSkillUse");
        StopCoroutine("GaugeIdle");
        OnPress();
    }

    public override void StopWeaponAction() //GetKeyUp
    {
        if(bLock) return;
        photonView.CustomRPC(this, "animPlay", RpcTarget.All, false);
        
        StopCoroutine("OnShieldSkillUse");
        StartCoroutine("GaugeIdle");
        OnCancle();
    }

    [PunRPC]
    public void animPlay(bool isStart)
    {
        //어떤상황에서 어떤애니메이션
        if (isStart)
        {
            anim.CrossFade("ShieldOn", 0.1f);
        }
        if (isStart == false && anim.GetCurrentAnimatorStateInfo(0).IsName("ShieldOn") == true)
        {
            anim.CrossFade("ShieldOff", 0.1f);
        }
    }

    public override void StartReload() // 연료를 0까지 사용했을 때
    {
        if (isReloading || bLock)
            return;

        StopWeaponAction();
        StartCoroutine(GaugeOver());
    }

    IEnumerator OnShieldSkillUse() // 쉴드스킬을 사용했을 때
    {
        while (true)
        {
            CurrentAmmo -= Time.deltaTime * gaugeDownSpeed;
            yield return null;
        }
    }

    IEnumerator GaugeIdle() // 쉴드스킬을 누르지 않은 상태일 때
    {
        while (true)
        {
            CurrentAmmo += Time.deltaTime * gaugeUpSpeed;
            yield return null;
        }
    }

    IEnumerator GaugeOver() // 과부하
    {
        isReloading = true;

        yield return new WaitForSeconds(3f); //3초에 패널티

        isReloading = false;
    }

    public bool TakeDamage(float damage)
    {
        CurrentAmmo -= damage;
        return true;
    }

}
