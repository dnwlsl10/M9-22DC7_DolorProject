using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public enum WeaponName {Basic, Shield, Missile, Orb, Laser}
public enum HandSide {Left, Right}
[System.Serializable]
public struct WeaponSetting
{
    public WeaponName weaponName;
    public InputActionProperty button;
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
    public HandSide handSide;
    protected float lastAttackTime = 0;
    protected bool isReloading;
    protected bool isAttacking;
    public bool bLock;

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
    public virtual void Initialize()
    {
        Debug.Log("init");
        lastAttackTime = -weaponSetting.attackRate;
        isReloading = false;
        isAttacking = false;
    }

    protected void OnEnable() {
        if (photonView.Mine == false) return;

        WeaponSystem.instance.RegistWeapon(weaponSetting.button.action, this);
        Initialize();
    }
    protected void OnDisable() {
        if (photonView.Mine)
            WeaponSystem.instance.UnregistWeapon(weaponSetting.button.action, this);
    }
}
