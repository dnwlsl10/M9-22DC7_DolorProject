using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum WeaponName {Basic, Shield, Missile, Grenade, Laser}
[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName;
    public int damage;
    public float currentAmmo;
    public float maxAmmo;
    public float attackRate;
    public float attackDistance;
}
public delegate void Cur_MaxEvent(float curValue, float maxValue);

public class WeaponBase : MonoBehaviourPun
{
    [SerializeField]
    protected AudioSource audioSource;
    public WeaponSetting weaponSetting;
    protected float lastAttackTime = 0;
    protected bool isReloading;
    protected bool isAttacking;
    public GrabEvent grapEvent;

    public virtual void StartWeaponAction(){}
    public virtual void StopWeaponAction(){}
    public virtual void StartReload(){}
    
    protected void PlaySound(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
    protected void Initialize()
    {
        lastAttackTime = -weaponSetting.attackRate;
        isReloading = false;
        isAttacking = false;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    protected void Awake()
    {
        Initialize();
    }
}
