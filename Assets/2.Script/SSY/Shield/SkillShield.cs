using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// === 방패의 기능 ===
// 적의 투사체 모든 것을 막는다.
// 방패의 체력
// 투사체 별 - 데미지

// 게임 시작 시 게이지 100
// Down
// LeftIndexTrigger 버튼을 누르면 방패 오브젝트가 켜지고
// 

// 애니메이션의 상태를 ShieldOn = Play
// 누르고 있으면 게이지가 초당 -1씩 깎이고
//

//떼면 게이지가 초당 1씩 회복되고
//애니메이션의 상태를 초기화

public class SkillShield : WeaponBase, IDamageable
{
    public event Cur_MaxEvent OnValueChange;

    public GameObject shieldCreatePos;
    public GameObject shield;
    Animator anim;

    public float CurrentAmmo
    {
        get { return weaponSetting.currentAmmo; }
        set
        {
            // curammo 1, value 1.1, maxammo 1
            float prevAmmo = weaponSetting.currentAmmo;

            if (value <= 0)
            {
                value = 0;
                weaponSetting.currentAmmo = value;

                weaponSetting.currentAmmo = 0;
            }
            else if (value > weaponSetting.maxAmmo)
            {
                value = weaponSetting.maxAmmo;
                weaponSetting.currentAmmo = value;

                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
            }
            else
            {
                weaponSetting.currentAmmo = value;
            }

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

    // void SetAmmo(float value)
    // {
    //     weaponSetting.currentAmmo = value;
    // }

    void Start()
    {
        anim = GetComponent<Animator>();
        shield.SetActive(false);
    }

    public override void StartWeaponAction() //GetKeyDown
    {

        anim.Play("ShieldOn");
        StartCoroutine("OnShieldSkillUse");
        StopCoroutine("GaugeIdle");

    }

    public override void StopWeaponAction() //GetKeyUp
    {
        anim.Play("ShieldOff");
        StopCoroutine("OnShieldSkillUse");
        StartCoroutine("GaugeIdle");
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
        CurrentAmmo -= Time.deltaTime;
        yield return null;
    }

    IEnumerator GaugeIdle() // 쉴드스킬을 누르지 않은 상태일 때
    {
        CurrentAmmo += Time.deltaTime;
        yield return null;
    }

    IEnumerator GaugeOver() // 과부하
    {
        isReloading = true;

        yield return new WaitForSeconds(3f); //3초에 패널티

        isReloading = false;
    }

    public void TakeDamage(float damage)
    {

    }
}
