using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectBasicWeapon : WeaponBase
{
    public event Cur_MaxEvent OnValueChange;
    [Header("ButtonHandler")]
    public ButtonHandler gripButton;
    public ButtonHandler shootButton;

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
            weaponSetting.currentAmmo = value;
            OnValueChange?.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
        }
    }

    public bool isAutomatic;
    IEnumerator coroutineHolder;

    private void OnEnable()
    {
        shootButton.OnButtonDown += StartWeaponAction;
        shootButton.OnButtonUp += StopWeaponAction;

    }
    private void OnDisable()
    {
        shootButton.OnButtonDown -= StartWeaponAction;
        shootButton.OnButtonUp -= StopWeaponAction;
    }

    public override void StartWeaponAction()
    {
        if (isReloading)
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
        while (true)
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

        RPCAttack(bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        if (CurrentAmmo <= 0)
            StartReload();
    }

    private void RPCAttack(Vector3 bulletPosition, Quaternion bulletRotation)
    {
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
        while (false /* until reload procedure finish */)
            yield return null;

        isReloading = false;
        CurrentAmmo = weaponSetting.maxAmmo;
    }
}
