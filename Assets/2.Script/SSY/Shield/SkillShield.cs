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
        handSide = HandSide.Left;
        gaugeUpSpeed = 10;
        gaugeDownSpeed = 20;
    }

    [SerializeField] float gaugeUpSpeed;
    [SerializeField] float gaugeDownSpeed;
    [SerializeField] Animator anim;
    bool shieldOn;
    public System.Action OnPress;
    public System.Action OnCancle;
    

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
        if (photonView.Mine == false) return;
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
        if (isReloading || bLock)
            return;
        
        if (shieldOn == false)
        {
            shieldOn = true;
            photonView.CustomRPC(this, "animPlay", RpcTarget.All, true);
            OnPress();
        }

    }

    public override void StopWeaponAction() //GetKeyUp
    {
        if (bLock) return;
        if (shieldOn == true)
        {
            shieldOn = false;
            photonView.CustomRPC(this, "animPlay", RpcTarget.All, false);
            OnCancle();
        }
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
        if (isReloading || bLock)
            return;

        // StopWeaponAction();
        StartCoroutine(GaugeOver());
    }

    IEnumerator GaugeOver() // 과부하
    {
        isReloading = true;
        WeaponSystem.instance.LockWeapon(weaponSetting.weaponName);

        yield return new WaitForSeconds(3f); //3초에 패널티

        WeaponSystem.instance.UnlockWeapon(weaponSetting.weaponName);
        isReloading = false;
    }

    public bool TakeDamage(float damage)
    {
        CurrentAmmo -= damage;
        return true;
    }

}
