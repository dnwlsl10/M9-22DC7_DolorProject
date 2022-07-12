#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;

[RequireComponent(typeof(PhotonView))]
public class BasicWeapon : WeaponBase, IInitialize
{
    public void Reset()
    {
#if UNITY_EDITOR
        weaponSetting.weaponName = WeaponName.Basic;
        weaponSetting.maxAmmo = 10;
        weaponSetting.attackDistance = 10;
        weaponSetting.attackRate = 0.2f;
        weaponSetting.damage = 1;
        weaponSetting.bLock = false;
        handSide = HandSide.Right;
        isAutomatic = true;

        if (bullet == null)
            bullet = Resources.Load("Projectile 10") as GameObject;

        if (bulletSpawnPoint == null)
        {
            Utility.GetBoneTransform(transform.root, HumanBodyBones.RightLowerArm, out Transform forearm);
            bulletSpawnPoint = Utility.FindChildContainsName(forearm, new string[]{"Basic", "basic"});
            if (bulletSpawnPoint == null)
            {
                bulletSpawnPoint = new GameObject("BasicWeapon").transform;
                bulletSpawnPoint.parent = forearm;
                bulletSpawnPoint.localPosition = new Vector3(0.00133327337f,0.0120280581f,0.00210149423f);
                bulletSpawnPoint.localEulerAngles = new Vector3(272.714539f,170.087479f,64.0593033f);
            }
        }
#endif
    }

    [Header("SpawnPoint")]
    public Transform bulletSpawnPoint;

    [Header("Fire Effects")]
    [SerializeField] GameObject muzzleFlashEffect;
    [SerializeField] private GameObject bullet;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip onFireSFX;
    [SerializeField] private AudioClip onReloadSFX;
    [SerializeField] private bool isAutomatic;
    private WaitForEndOfFrame eof = new WaitForEndOfFrame();
    IEnumerator coroutineHolder;

    public float CurrentAmmo
    {
        get { return weaponSetting.currentAmmo;}
        set
        {
            float prevAmmo = weaponSetting.currentAmmo;
            weaponSetting.currentAmmo = Mathf.Clamp(value, 0, weaponSetting.maxAmmo);

            if (prevAmmo != weaponSetting.currentAmmo)
            {
                ValueChangeEvent(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
            }
        }
    }
    
    public override void Initialize()
    {
        base.Initialize();
        CurrentAmmo = weaponSetting.maxAmmo;
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

        WeaponSystem.instance.StartActionCallback((int)weaponSetting.weaponName);
    }

    public override void StopWeaponAction()
    {
        if (coroutineHolder != null)
            StopCoroutine(coroutineHolder);

        WeaponSystem.instance.StopActionCallback((int)weaponSetting.weaponName);
    }

    public override void StartReload()
    {
        if (isReloading) 
            return;

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

        
        photonView.CustomRPC(this, "RPCAttack", RpcTarget.All, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        if (CurrentAmmo <= 0)
            StartReload();
    }

    [PunRPC]
    private void RPCAttack(Vector3 bulletPosition, Quaternion bulletRotation)
    {
        if (photonView.Mine)
        {
            var bt = NetworkObjectPool.instance.SpawnFromPool<Bullet>(bullet.name, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bt.SetAttackDistance(weaponSetting.attackDistance);
        }

        StartCoroutine(OnMuzzleFlashEffect());
        if (onFireSFX) AudioPool.instance.Play(onFireSFX.name, 2, bulletPosition);
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
        StopWeaponAction();

        yield return new WaitForSeconds(2f); // Reloading Procedure

        isReloading = false;
        // WeaponSystem.instance.UnlockWeapon(weaponSetting.weaponName);
        CurrentAmmo = weaponSetting.maxAmmo;
    }
}
