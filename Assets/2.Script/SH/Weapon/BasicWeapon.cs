#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;

[RequireComponent(typeof(PhotonView))]
public class BasicWeapon : WeaponBase
{
    public event Cur_MaxEvent OnValueChange;
    [Header("Button")]
    public UnityEngine.InputSystem.InputActionReference gripButton;
    public UnityEngine.InputSystem.InputActionReference shootButton;

    [Header("SpawnPoint")]
    public Transform bulletSpawnPoint;

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;
    [SerializeField]
    private GameObject bullet;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip onFireSFX;
    [SerializeField]
    private AudioClip onReloadSFX;
    private WaitForEndOfFrame eof = new WaitForEndOfFrame();
    public float CurrentAmmo
    {
        get { return weaponSetting.currentAmmo;}
        set
        {
            float prevAmmo = weaponSetting.currentAmmo;
            weaponSetting.currentAmmo = Mathf.Clamp(value, 0, weaponSetting.maxAmmo);

            if (prevAmmo != weaponSetting.currentAmmo)
            {
                OnValueChange?.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
            }
        }
    }
    
    public bool isAutomatic;
    IEnumerator coroutineHolder;

    private void OnEnable()
    {
        print("SingleMode :" + PhotonNetwork.SingleMode);
        if (photonView.Mine == false) return;

        shootButton.action.started += StartWeaponEvent;
        shootButton.action.canceled += StopWeaponEvent;
        gripButton.action.canceled += StopWeaponEvent;
    }
    private void OnDisable()
    {
        if (photonView.Mine == false) return;
        
        shootButton.action.started -= StartWeaponEvent;
        shootButton.action.canceled -= StopWeaponEvent;
        gripButton.action.canceled -= StopWeaponEvent;
    }
    public void StartWeaponEvent(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        StartWeaponAction();
    }
    public void StopWeaponEvent(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        StopWeaponAction();
    }

    public override void StartWeaponAction()
    {
        if (isReloading && !grapEvent.isRightGrab)
            return;

        if (isAutomatic)
        {
            coroutineHolder = ContinuousFire();
            StartCoroutine(coroutineHolder);
        }
        else
            OnAttack();
    }

    public override void StopWeaponAction()
    {
        if (coroutineHolder != null)
            StopCoroutine(coroutineHolder);
    }

    public override void StartReload()
    {
        if (isReloading) 
            return;

        StopWeaponAction();
        StartCoroutine(OnReload());
    }

    IEnumerator ContinuousFire()
    {
        while(true)
        {
            yield return eof;
            OnAttack();
        }
    }

    private void OnAttack()
    {
        // if not ready-to-shoot, return
        if (Time.time - lastAttackTime < weaponSetting.attackRate || CurrentAmmo <= 0)
            return;
        
        lastAttackTime = Time.time;
        CurrentAmmo--;

        photonView.CustomRPC(this, "RPCAttack", RpcTarget.AllViaServer, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        if (CurrentAmmo <= 0)
            StartReload();
    }

    [PunRPC]
    private void RPCAttack(Vector3 bulletPosition, Quaternion bulletRotation)
    {
        if (photonView.Mine)
            NetworkObjectPool.SpawnFromPool(bullet.name, bulletPosition, bulletRotation);
        
        StartCoroutine(OnMuzzleFlashEffect());
        PlaySound(onFireSFX);
    }

    IEnumerator OnMuzzleFlashEffect()
    {
        if (muzzleFlashEffect == null) yield break;

        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.6f);
        muzzleFlashEffect.SetActive(false);
    }

    IEnumerator OnReload()
    {
        isReloading = true;

        yield return new WaitForSeconds(2f);
        while(false /* until reload procedure finish */)
            yield return null;

        isReloading = false;

        CurrentAmmo = weaponSetting.maxAmmo;
    }
}
