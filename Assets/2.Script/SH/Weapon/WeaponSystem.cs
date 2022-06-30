#define test
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class WeaponSystem : MonoBehaviourPun, IInitialize
{

    [ContextMenu("Initialize")]
    public void Reset() {
#if UNITY_EDITOR
        IKWeight weight = transform.root.GetComponentInChildren<IKWeight>();
        DestroyImmediate(transform.root.GetComponentInChildren<GrabEvent>());

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
            }
        }

        if (input_name_pair == null) input_name_pair = new SDictionaty();

        if (input_name_pair.ContainsValue(WeaponName.Basic) == false)
            input_name_pair.Add(Utility.CloneAction(ActionMap.XRI_RightHand_Interaction, "Activate"), WeaponName.Basic);
        if (input_name_pair.ContainsValue(WeaponName.Shield) == false)
            input_name_pair.Add(Utility.CloneAction(ActionMap.XRI_LeftHand_Interaction, "Activate"), WeaponName.Shield);
        if(input_name_pair.ContainsValue(WeaponName.Missile) == false)
            input_name_pair.Add(Utility.CloneAction(ActionMap.XRI_RightHand_Interaction, "GuidedMissile"), WeaponName.Missile);
        
#if test
        grabL = Utility.FindInputReference(ActionMap.XRI_LeftHand_Interaction, "Select");
        grabR = Utility.FindInputReference(ActionMap.XRI_RightHand_Interaction, "Select");
#endif
#endif
    }

    [System.Serializable]
    public class SDictionaty : RotaryHeart.Lib.SerializableDictionary.SerializableDictionaryBase<InputAction, WeaponName>{}
    public SDictionaty input_name_pair;

    WeaponBase[] weapons;
    List<int>[] weaponIndex_byHand;
    public bool[] canUseSkill;
    private bool[] isGrabbing;
    private bool[] usingSkill;

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

    public void OnGrabRight(bool grabbing) => SetGrabState((int)HandSide.Left, grabbing);
    public void OnGrabLeft(bool grabbing) => SetGrabState((int)HandSide.Right, grabbing);

    private void Awake() 
    {
        if (photonView.Mine == false)
        {
            input_name_pair = null;
            Destroy(this);
        }

        weaponIndex_byHand = new List<int>[2];
        weaponIndex_byHand[0] = new List<int>();
        weaponIndex_byHand[1] = new List<int>();
        isGrabbing = new bool[2];
        usingSkill = new bool[2];

        int weaponNameCount = System.Enum.GetValues(typeof(WeaponName)).Length;
        canUseSkill = new bool[weaponNameCount];
        for(int i = 0; i < weaponNameCount; i++) canUseSkill[i] = true;
        weapons = new WeaponBase[weaponNameCount];
        

        foreach (var weapon in transform.root.GetComponentsInChildren<WeaponBase>())
        {
            int index = (int)weapon.weaponSetting.weaponName;
            weapons[index] = weapon;
            weaponIndex_byHand[(int)weapon.handSide].Add(index);
        }
        
        input_name_pair = null;
    }

    void StartWeaponEvent(InputAction.CallbackContext ctx)
    {
        int index = (int)input_name_pair[ctx.action];
        if (canUseSkill[index] == false) return;

        WeaponBase weapon = weapons[index];
        index = (int)weapon.handSide;
        
        if (isGrabbing[index] == true && usingSkill[index] == false)
        {
            usingSkill[index] = true;
            weapon.StartWeaponAction();
        }
        else
            print("Cannot use skill");
    }

    public void StopWeaponEvent(WeaponName weaponName) => StopWeaponEvent((int)weaponName);
    private void StopWeaponEvent(InputAction.CallbackContext ctx) => StopWeaponEvent((int)input_name_pair[ctx.action]);
    private void StopWeaponEvent(int index)
    {
        WeaponBase weapon = weapons[index];
        weapon.StopWeaponAction();
        usingSkill[(int)weapon.handSide] = false;
    }

#if test
    [Header("Test")]
    public InputActionReference grabL;
    public InputActionReference grabR;
    private void OnGrabLeft(InputAction.CallbackContext ctx) => OnGrabLeft(ctx.ReadValueAsButton());
    private void OnGrabRight(InputAction.CallbackContext ctx) => OnGrabRight(ctx.ReadValueAsButton());
#endif

    private void OnEnable() {
        if (photonView.Mine == false) return;

        foreach(var key_val in input_name_pair)
        {
            key_val.Key.Enable();
            key_val.Key.started += StartWeaponEvent;
            key_val.Key.canceled += StopWeaponEvent;
        }

        #if test
        Debug.LogWarning("WeaponSystem is in testMode");
        if (Utility.isVRConnected == false)
        {
            grabL.action.started += OnGrabLeft;
            grabR.action.started += OnGrabRight;
            grabL.action.canceled += OnGrabLeft;
            grabR.action.canceled += OnGrabRight;
        } 
        #endif
    }
    private void OnDisable() {
        if (photonView.Mine == false) return;

        foreach(var key_val in input_name_pair)
        {
            key_val.Key.started -= StartWeaponEvent;
            key_val.Key.canceled -= StopWeaponEvent;
            key_val.Key.Disable();
        }

        #if test
        if (Utility.isVRConnected == false)
        {
            grabL.action.started -= OnGrabLeft;
            grabR.action.started -= OnGrabRight;
            grabL.action.canceled -= OnGrabLeft;
            grabR.action.canceled -= OnGrabRight;
        }
        #endif
    }
}
