using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    public float gaugeUpSpeed;
    public float gaugeDownSpeed;
    Animator anim;
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
        if (isReloading)
            return;
        if (PhotonNetwork.SingleMode == false)
        {
            photonView.RPC("animPlay", RpcTarget.All, true);
        }
        else
        {
            animPlay(true);
        }
        StartCoroutine("OnShieldSkillUse");
        StopCoroutine("GaugeIdle");
    }

    public override void StopWeaponAction() //GetKeyUp
    {
        if(PhotonNetwork.SingleMode == false)
        {
        photonView.RPC("animPlay", RpcTarget.All, false);
        }
        else
        {
        animPlay(false);
        }
        
        StopCoroutine("OnShieldSkillUse");
        StartCoroutine("GaugeIdle");
    }

    [PunRPC]
    public void animPlay(bool isStart)
    {
        //어떤상황에서 어떤애니메이션
        if (isStart)
        {
            anim.Play("ShieldOn");
        }
        if (isStart == false)
        {
            anim.Play("ShieldOff");
        }
    }

    public override void StartReload() // 연료를 0까지 사용했을 때
    {
        if (isReloading)
            return;
        print("LLL");

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
