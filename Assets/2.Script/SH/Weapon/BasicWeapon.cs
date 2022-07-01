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
    public event Cur_MaxEvent OnValueChange;

    public System.Action OnPress;
    public System.Action OnCancle;

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
    public override void Initialize() => CurrentAmmo = 10;
    
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

        OnPress();
    }

    public override void StopWeaponAction()
    {
        if (coroutineHolder != null)
            StopCoroutine(coroutineHolder);

        OnCancle();
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
        if (photonView.cachedMine)
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
