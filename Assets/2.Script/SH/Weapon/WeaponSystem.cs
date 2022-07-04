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

    private Dictionary<InputAction, int> input_name_pair;

    WeaponBase[] weapons;
    List<int>[] weaponIndex_byHand;
    private bool[] canUseSkill;
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

    public static WeaponSystem instance;
    private void Awake() 
    {
        if (photonView.Mine == false)
        {
            input_name_pair = null;
            Destroy(this);
        }
        else 
        {
            if (instance) Destroy(instance);
            instance = this;
            InitLocal();
        }
    }

    void InitLocal()
    {
        input_name_pair = new Dictionary<InputAction, int>();

        weaponIndex_byHand = new List<int>[2];
        weaponIndex_byHand[0] = new List<int>();
        weaponIndex_byHand[1] = new List<int>();
        isGrabbing = new bool[2];
        usingSkill = new bool[2];

        int weaponNameCount = System.Enum.GetNames(typeof(WeaponName)).Length;
        canUseSkill = new bool[weaponNameCount];
        for(int i = 0; i < weaponNameCount; i++) canUseSkill[i] = true;
        weapons = new WeaponBase[weaponNameCount];
    }

    void StartWeaponEvent(InputAction.CallbackContext ctx)
    {
        int index = (int)input_name_pair[ctx.action];
        if (canUseSkill[index] == false) return;
        print("Start Weapon Evnet " + ((WeaponName)index).ToString());

        WeaponBase weapon = weapons[index];
        int handIndex = (int)weapon.handSide;
        if (isGrabbing[handIndex] == true && usingSkill[handIndex] == false)
        {
            usingSkill[handIndex] = true;
            weapon.StartWeaponAction();
        }
        else
            print("Cannot use skill");
    }

    public void LockWeapon(WeaponName weaponName)
    {
        canUseSkill[(int)weaponName] = false;
        StopWeaponEvent((int)weaponName);
    }
    public void UnlockWeapon(WeaponName weaponName)
    {
        canUseSkill[(int)weaponName] = true;
    }
    private void StopWeaponEvent(InputAction.CallbackContext ctx) => StopWeaponEvent((int)input_name_pair[ctx.action]);
    private void StopWeaponEvent(int index)
    {
        print("Stop Weapon Evnet " + ((WeaponName)index).ToString());
        WeaponBase weapon = weapons[index];
        weapon.StopWeaponAction();
        usingSkill[(int)weapon.handSide] = false;
    }

    public void RegistWeapon(InputAction action, WeaponBase weapon)
    {  
        int weaponIndex = (int)weapon.weaponSetting.weaponName;

        if (input_name_pair.ContainsKey(action) == false)
        {
            input_name_pair.Add(action, weaponIndex);
            weapons[weaponIndex] = weapon;
            weaponIndex_byHand[(int)weapon.handSide].Add(weaponIndex);

            action.started += StartWeaponEvent;
            action.canceled += StopWeaponEvent;
        }
    }

    public void UnregistWeapon(InputAction action, WeaponBase weapon)
    {
        if (input_name_pair.TryGetValue(action, out var weaponName))
        {
            input_name_pair.Remove(action);
            weapons[weaponName] = null;
            weaponIndex_byHand[(int)weapon.handSide].Remove(weaponName);
        
            action.started -= StartWeaponEvent;
            action.canceled -= StopWeaponEvent;
        }
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
