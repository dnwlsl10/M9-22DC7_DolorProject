using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SkillShield : WeaponBase, IDamageable
{
    public event Cur_MaxEvent OnValueChange;

    public GameObject shieldCreatePos;
    public GameObject shield;
    public float gaugeUpSpeed;
    public float gaugeDownSpeed;
    Animator anim;
    public bool isLinked = false;
    public bool isOnSkillUse = false;
    public UnityEngine.InputSystem.InputActionReference alpha1;

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

    private void OnEnable()
    {
        if (photonView.Mine == false) return;

        alpha1.action.started += StartEvent;
        alpha1.action.canceled += StopEvent;
    }
    private void OnDisable()
    {
        if (photonView.Mine == false) return;

        alpha1.action.started -= StartEvent;
        alpha1.action.canceled -= StopEvent;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void StartEvent(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        StartWeaponAction();
    }
    void StopEvent(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        StopWeaponAction();
    }

    public override void StartWeaponAction() //GetKeyDown
    {
        if (isReloading || isLinked == true)
            return;
        photonView.CustomRPC(this, "animPlay", RpcTarget.All, true);
        StartCoroutine("OnShieldSkillUse");
        StopCoroutine("GaugeIdle");
    }

    public override void StopWeaponAction() //GetKeyUp
    {

        photonView.CustomRPC(this, "animPlay", RpcTarget.All, false);
        
        StopCoroutine("OnShieldSkillUse");
        StartCoroutine("GaugeIdle");
    }

    [PunRPC]
    public void animPlay(bool isStart)
    {
        //어떤상황에서 어떤애니메이션
        if (isStart)
        {
            anim.CrossFade("ShieldOn", 0.1f);
        }
        if (isStart == false)
        {
            anim.CrossFade("ShieldOff", 0.1f);
        }
    }

    public override void StartReload() // 연료를 0까지 사용했을 때
    {
        if (isReloading)
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

    public void TakeDamage(float damage)
    {
        CurrentAmmo -= damage;
    }

}
