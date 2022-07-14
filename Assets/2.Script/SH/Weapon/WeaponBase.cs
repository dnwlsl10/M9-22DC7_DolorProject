using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public enum WeaponName {Basic, Shield, Missile, Orb}
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
    public bool bLock;
}

public class WeaponBase : MonoBehaviourPun
{
    public event System.Action<float, float> OnValueChange;
    public WeaponSetting weaponSetting;
    public HandSide handSide;
    protected float lastAttackTime = 0;
    protected bool isReloading;
    protected bool isAttacking;

    protected void StartWeaponAction(InputAction.CallbackContext ctx) => WeaponSystem.instance.TryUseWeapon((int)weaponSetting.weaponName, (int)handSide, StartWeaponAction);
    protected void StopWeaponAction(InputAction.CallbackContext ctx) => StopWeaponAction();
    public virtual void StartWeaponAction(){}
    public virtual void StopWeaponAction(){}
    public virtual void StartReload(){}

    public virtual void Initialize()
    {
        // Debug.Log("init");
        lastAttackTime = -weaponSetting.attackRate;
        isReloading = false;
        isAttacking = false;
    }

    protected void OnEnable() {
        if (photonView.Mine)
        {
            weaponSetting.button.action.started += StartWeaponAction;
            weaponSetting.button.action.canceled += StopWeaponAction;
            Initialize();
        }
    }
    protected void OnDisable() {
        if (photonView.Mine)
        {
            weaponSetting.button.action.started -= StartWeaponAction;
            weaponSetting.button.action.canceled -= StopWeaponAction;
        }
    }

    protected void ValueChangeEvent(float cur, float max)
    {
        OnValueChange?.Invoke(cur, max);
    }
}
