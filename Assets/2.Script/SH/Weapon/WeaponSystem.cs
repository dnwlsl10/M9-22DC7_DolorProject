#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using Photon.Pun;
using System;

[RequireComponent(typeof(PhotonView))]
public class WeaponSystem : MonoBehaviourPun, IInitialize
{
    [ContextMenu("Initialize")]
    public void Reset() {
#if UNITY_EDITOR
        IKWeight weight = transform.root.GetComponentInChildren<IKWeight>();
        DestroyImmediate(transform.root.GetComponentInChildren<GrabEvent>());
        UIShield uIShield = transform.root.GetComponentInChildren<UIShield>();
        UIBasicWeapon uIBasicWeapon = transform.root.GetComponentInChildren<UIBasicWeapon>();
        UIGuidedMissile uIGuidedMissile = transform.root.GetComponentInChildren<UIGuidedMissile>();
        UIOrb uIOrb = transform.root.GetComponentInChildren<UIOrb>();

        Transform tr = Utility.FindChildMatchName(transform.root, "Cockpit");
        if (tr != null)
        foreach (var grababble in tr.GetComponentsInChildren<Autohand.Grabbable>())
        {
            if (grababble.handType == Autohand.HandType.left)
            {
                grababble.onGrab = new Autohand.UnityHandGrabEvent();
                grababble.onRelease = new Autohand.UnityHandGrabEvent();
                
                var hilight = grababble.GetComponentInChildren<GrabControllerHighlight>();

                
                UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(grababble.onGrab, OnGrabLeft, true);
                UnityEditor.Events.UnityEventTools.AddIntPersistentListener(grababble.onGrab, weight.OnLeftGripEvent, 1);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onGrab, hilight.OnGrabHightlight);

                UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(grababble.onRelease, OnGrabLeft, false);
                UnityEditor.Events.UnityEventTools.AddIntPersistentListener(grababble.onRelease, weight.OnLeftGripEvent, 0);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onRelease, hilight.OnRelease);

                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onGrab, uIShield.OnFirstButton);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onRelease, uIShield.OFFFirstButton);


            }
            else if (grababble.handType == Autohand.HandType.right)
            {
                grababble.onGrab = new Autohand.UnityHandGrabEvent();
                grababble.onRelease = new Autohand.UnityHandGrabEvent();

                var hilight = grababble.GetComponentInChildren<GrabControllerHighlight>();
                
                UnityEditor.Events.UnityEventTools.AddIntPersistentListener(grababble.onGrab, weight.OnRightGripEvent, 1);
                UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(grababble.onGrab, OnGrabRight, true);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onGrab, hilight.OnGrabHightlight);

                UnityEditor.Events.UnityEventTools.AddIntPersistentListener(grababble.onRelease, weight.OnRightGripEvent, 0);
                UnityEditor.Events.UnityEventTools.AddBoolPersistentListener(grababble.onRelease, OnGrabRight, false);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onRelease, hilight.OnRelease);

                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onGrab, uIBasicWeapon.OnFirstButton);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onRelease, uIBasicWeapon.OFFFirstButton);

                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onGrab, uIGuidedMissile.OnFirstButton);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onRelease, uIGuidedMissile.OFFFirstButton);

                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onGrab, uIOrb.OnFirstButton);
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(grababble.onRelease, uIOrb.OFFFirstButton);
            }
        }
        
#if test
        grabL = Utility.FindInputReference(ActionMap.XRI_LeftHand_Interaction, "Select");
        grabR = Utility.FindInputReference(ActionMap.XRI_RightHand_Interaction, "Select");
#endif
#endif
    }

    public static WeaponSystem instance;
    [SerializeField] WeaponBase[] weapons;
    List<int>[] weaponIndex_byHand;
    private bool[] canUseSkill;
    Action[] onStartAction, onStopAction;
    [SerializeField] private bool[] isGrabbing;
    [SerializeField] private bool[] usingSkill;

    void SetGrabState(int index, bool value)
    {
        if (isGrabbing[index] == value) return;

        if (value == false)
        {
            foreach (int i in weaponIndex_byHand[index])
                weapons[i].StopWeaponAction();
            usingSkill[index] = false;
        }

        isGrabbing[index] = value;
    }

    public void OnGrabLeft(bool grabbing) => SetGrabState((int)HandSide.Left, grabbing);
    public void OnGrabRight(bool grabbing) => SetGrabState((int)HandSide.Right, grabbing);

    private void Awake() 
    {
        // print(photonView.Mine);
        if (photonView.Mine == false)
        {
            Destroy(this);
        }
        else 
        {
            if (instance) Destroy(instance);
            instance = this;
            InitLocal();
        }
    }

    private void ArrangeWeaponIndex()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == (int)weapons[i].weaponSetting.weaponName)
            {
                InitSetting(weapons[i]);
                continue;
            }

            for (int j = i+1; j < weapons.Length; j++)
            {
                if (i == (int)weapons[j].weaponSetting.weaponName)
                {
                    var tmp = weapons[i];
                    weapons[i] = weapons[j];
                    weapons[j] = tmp;

                    InitSetting(weapons[i]);
                }
            }
        }
    }

    private void InitSetting(WeaponBase wb)
    {
        int index = (int)wb.weaponSetting.weaponName;
        weaponIndex_byHand[(int)wb.handSide].Add(index);
        canUseSkill[index] = true;
    }

    public void AddStartStopCallbackTarget(IWeaponEvent intfce, int weaponIndex)
    {
        onStartAction[weaponIndex] += intfce.OnSecondButton;
        onStopAction[weaponIndex] += intfce.OffSecondButton;
        weapons[weaponIndex].OnValueChange += intfce.EventValue;
    }
    public void RemoveStartStopCallbackTarget(IWeaponEvent intfce, int weaponIndex)
    {
        onStartAction[weaponIndex] -= intfce.OnSecondButton;
        onStopAction[weaponIndex] -= intfce.OffSecondButton;
        weapons[weaponIndex].OnValueChange -= intfce.EventValue;
    }

    void InitLocal()
    {
        int weaponNameCount = System.Enum.GetNames(typeof(WeaponName)).Length;

        weaponIndex_byHand = new List<int>[2];
        weaponIndex_byHand[0] = new List<int>();
        weaponIndex_byHand[1] = new List<int>();
        usingSkill = new bool[2];
        isGrabbing = new bool[2];

        onStartAction = new Action[weaponNameCount];
        onStopAction = new Action[weaponNameCount];
        canUseSkill = new bool[weaponNameCount];

        ArrangeWeaponIndex();
    }

    public void StartActionCallback(int weaponIndex)
    {
        onStartAction[weaponIndex]?.Invoke();
        usingSkill[(int)weapons[weaponIndex].handSide] = true;
    }
    public void StopActionCallback(int weaponIndex)
    {
        onStopAction[weaponIndex]?.Invoke();
        usingSkill[(int)weapons[weaponIndex].handSide] = false;
    }

    public void TryUseWeapon(int weaponIndex, int handSide, Action action)
    {
        if (isGrabbing[handSide] == true && usingSkill[handSide] == false && canUseSkill[weaponIndex] == true)
        {
            action();
        }
        else
        {
            // Debug.Log("Cannot Use Weapon");
        }
    }

    public void LockWeapon(WeaponName weaponName)
    {
        canUseSkill[(int)weaponName] = false;
        weapons[(int)weaponName].StopWeaponAction();
    }
    public void UnlockWeapon(WeaponName weaponName)
    {
        canUseSkill[(int)weaponName] = true;
    }

#if test
    [Header("Test")]
    public InputActionReference grabL;
    public InputActionReference grabR;
    private void OnGrabLeft(InputAction.CallbackContext ctx) => OnGrabLeft(ctx.ReadValueAsButton());
    private void OnGrabRight(InputAction.CallbackContext ctx) => OnGrabRight(ctx.ReadValueAsButton());

    private void OnEnable() {
        if (photonView.Mine == false) return;
        Debug.LogWarning("WeaponSystem is in testMode");
        if (Utility.isVRConnected == false)
        {
            grabL.action.started += OnGrabLeft;
            grabR.action.started += OnGrabRight;
            grabL.action.canceled += OnGrabLeft;
            grabR.action.canceled += OnGrabRight;
        }
    }
    private void OnDisable() {
        if (photonView.Mine == false) return;

        if (Utility.isVRConnected == false)
        {
            grabL.action.started -= OnGrabLeft;
            grabR.action.started -= OnGrabRight;
            grabL.action.canceled -= OnGrabLeft;
            grabR.action.canceled -= OnGrabRight;
        }
    }
#endif
}
