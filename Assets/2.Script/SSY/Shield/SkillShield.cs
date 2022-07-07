using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class SkillShield : WeaponBase, IDamageable
{
    public event Cur_MaxEvent OnValueChange;
    public void Reset()
    {
        weaponSetting.weaponName = WeaponName.Shield;
        weaponSetting.maxAmmo = 100;
        weaponSetting.bLock = false;
        handSide = HandSide.Left;
        gaugeUpSpeed = 10;
        gaugeDownSpeed = 20;
    }

    [SerializeField] float gaugeUpSpeed;
    [SerializeField] float gaugeDownSpeed;
    [SerializeField] Animator anim;
    bool shieldOn;
    

    // public UnityEngine.InputSystem.InputActionReference alpha1;

    public float CurrentAmmo
    {
        get { return weaponSetting.currentAmmo; }
        set
        {
            if (isReloading) return;

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

    public override void Initialize()
    {
        base.Initialize();
        anim.Play("ShieldOff", 0, 1);
        CurrentAmmo = weaponSetting.maxAmmo;
    }

    private void Update() {
        if (photonView.Mine == false || weaponSetting.bLock) return;
        float deltaTime = Time.deltaTime;

        if (shieldOn)
        {
            CurrentAmmo -= deltaTime * gaugeDownSpeed;
        }
        else
        {
            CurrentAmmo += deltaTime * gaugeUpSpeed;
        }
    }
    
    public override void StartWeaponAction() //GetKeyDown
    {
        if (isReloading || CurrentAmmo == 0)
            return;
        
        if (shieldOn == false)
        {
            shieldOn = true;
            photonView.CustomRPC(this, "animPlay", RpcTarget.All, true);
            WeaponSystem.instance.StartActionCallback((int)weaponSetting.weaponName);
        }

    }

    public override void StopWeaponAction() //GetKeyUp
    {
        if (shieldOn == true)
        {
            shieldOn = false;
            photonView.CustomRPC(this, "animPlay", RpcTarget.All, false);
            WeaponSystem.instance.StopActionCallback((int)weaponSetting.weaponName);
        }
    }

    GameObject sound;

    [PunRPC]
    public void animPlay(bool isStart)
    {
        print(isStart);
        //어떤상황에서 어떤애니메이션
        if (isStart)
        {
            anim.CrossFade("ShieldOn", 0.1f);
            sound = AudioPool.instance.Play("264061__paul368__sfx-door-open", 1, anim.transform.position, anim.transform);
        }
        if (isStart == false)
        {
            anim.CrossFade("ShieldOff", 0.1f);
            sound?.SetActive(false);
        }
    }

    public override void StartReload() // 연료를 0까지 사용했을 때
    {
        if (isReloading || weaponSetting.bLock)
            return;

        StopWeaponAction();
        StartCoroutine(GaugeOver());
    }

    IEnumerator GaugeOver() // 과부하
    {
        isReloading = true;
        // WeaponSystem.instance.LockWeapon(weaponSetting.weaponName);

        yield return new WaitForSeconds(3f); //3초에 패널티

        // WeaponSystem.instance.UnlockWeapon(weaponSetting.weaponName);
        isReloading = false;
    }

    public void TakeDamage(float damage)
    {
        photonView.CustomRPC(this, "ShieldD", photonView.Owner, damage);
    }

    [PunRPC]
    private void ShieldD(float damage)
    {
        Debug.Log("Shield Attacked " + damage);
        CurrentAmmo -= damage;
    }

}
