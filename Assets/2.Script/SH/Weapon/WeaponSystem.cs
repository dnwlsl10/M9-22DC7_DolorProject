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

        foreach (var grababble in Utility.FindChildMatchName(transform.root, "Cockpit").GetComponentsInChildren<Autohand.Grabbable>())
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

        ButtonMap basic;
        basic.weaponName = WeaponName.Basic;
        basic.button = Utility.FindInputReference(ActionMap.XRI_RightHand_Interaction, "Activate");
        if (buttonMaps.Contains(basic) == false)
            buttonMaps.Add(basic);
        
        ButtonMap shield;
        shield.weaponName = WeaponName.Shield;
        shield.button = Utility.FindInputReference(ActionMap.XRI_LeftHand_Interaction, "Activate");
        if (buttonMaps.Contains(shield) == false)
            buttonMaps.Add(shield);
#if test
        grabL = Utility.FindInputReference(ActionMap.XRI_LeftHand_Interaction, "Select");
        grabR = Utility.FindInputReference(ActionMap.XRI_RightHand_Interaction, "Select");
#endif
#endif
    }


    [System.Serializable]
    public struct ButtonMap
    {
        public WeaponName weaponName;
        public InputActionReference button;
    }

    List<WeaponBase> leftHandWeapon;
    List<WeaponBase> rightHandWeapon;
    private bool[] canUseSkill;
    private bool isRightGrab;
    private bool isLeftGrab;
    private bool usingRight;
    private bool usingLeft;
    bool grabRight
    {
        get { return isRightGrab; }
        set
        {
            if (isRightGrab == value) return;
            
            if (value == false)
                foreach (var weapon in rightHandWeapon)
                    weapon.StopWeaponAction();
            
            isRightGrab = value;
        }
    }
    bool grabLeft
    {
        get { return isLeftGrab; }
        set
        {
            if (isLeftGrab == value) return;
            
            if (value == false)
                foreach (var weapon in leftHandWeapon)
                    weapon.StopWeaponAction();

            isLeftGrab = value;
        }
    }

    public void OnGrabRight(bool grabbing) => grabRight = grabbing;
    public void OnGrabLeft(bool grabbing) => grabLeft = grabbing;
    public List<ButtonMap> buttonMaps;
    Dictionary<InputAction, WeaponBase> buttonWeaponPair;

    private void Awake() 
    {
        if (photonView.Mine == false) return;

        int weaponNameCount = System.Enum.GetValues(typeof(WeaponName)).Length;
        canUseSkill = new bool[weaponNameCount];
        for(int i = 0; i < weaponNameCount; i++) canUseSkill[i] = true;
        
        leftHandWeapon = new List<WeaponBase>();
        rightHandWeapon = new List<WeaponBase>();
        buttonWeaponPair = new Dictionary<InputAction, WeaponBase>();
        foreach (var weapon in transform.root.GetComponentsInChildren<WeaponBase>())
        {
            foreach(var map in buttonMaps)
                if (map.weaponName == weapon.weaponSetting.weaponName)
                {
                    buttonWeaponPair.Add(map.button.action, weapon);
                    break;
                }

            (weapon.handSide == HandSide.Left ? leftHandWeapon : rightHandWeapon).Add(weapon);
        }
    }
#if test
    [Header("Test")]
    public InputActionReference grabL;
    public InputActionReference grabR;
    public bool testMode;
    private void OnGrabLeft(InputAction.CallbackContext ctx) => OnGrabLeft(ctx.ReadValueAsButton());
    private void OnGrabRight(InputAction.CallbackContext ctx) => OnGrabRight(ctx.ReadValueAsButton());
#endif
    private void OnEnable() {
        if (photonView.Mine == false) return;

#if test
        Debug.LogWarning("WeaponSystem is in testMode");
        if (testMode == true)
        {
            grabL.action.started += OnGrabLeft;
            grabR.action.started += OnGrabRight;
            grabL.action.canceled += OnGrabLeft;
            grabR.action.canceled += OnGrabRight;
        }
#endif

        foreach(var weapon in buttonMaps)
        {
            weapon.button.action.started += StartWeaponEvent;
            weapon.button.action.canceled += StopWeaponEvent;
        }
    }
    private void OnDisable() {
        if (photonView.Mine == false) return;

#if test
        if (testMode == true)
        {
            grabL.action.started -= OnGrabLeft;
            grabR.action.started -= OnGrabRight;
            grabL.action.canceled -= OnGrabLeft;
            grabR.action.canceled -= OnGrabRight;
        }
#endif

        foreach(var weapon in buttonMaps)
        {
            weapon.button.action.started -= StartWeaponEvent;
            weapon.button.action.canceled -= StopWeaponEvent;
        }
    }

    void StartWeaponEvent(InputAction.CallbackContext ctx)
    {
        if (buttonWeaponPair.TryGetValue(ctx.action, out WeaponBase weaponBase))
        {
            if (canUseSkill[(int)weaponBase.weaponSetting.weaponName] == false) return;

            if (weaponBase.handSide == HandSide.Left)
            {
                if (grabLeft == true && usingLeft == false)
                {
                    usingLeft = true;
                    weaponBase.StartWeaponAction();
                }
                else
                    print("Not Grabbing");
            }
            else
            {
                if (grabRight == true && usingRight == false)
                {
                    usingRight = true;
                    weaponBase.StartWeaponAction();
                }
                else
                    print("Not Grabbing");
            }
        }
    }
    void StopWeaponEvent(InputAction.CallbackContext ctx)
    {
        if (buttonWeaponPair.TryGetValue(ctx.action, out WeaponBase weaponBase))
            weaponBase.StopWeaponAction();
            if (weaponBase.handSide == HandSide.Left)
            {
                usingLeft = false;
            }
            else
            {
                usingRight = false;
            }
    }
}
