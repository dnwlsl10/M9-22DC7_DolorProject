using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Photon.Pun;

public class MechMovementController : MonoBehaviourPun, IInitialize
{
    public void Reset() {
#if UNITY_EDITOR
        centerEye = GetComponentInChildren<Camera>(true)?.transform;
        leftHandJoystick = Utility.FindInputReference(ActionMap.XRI_LeftHand_Locomotion, "Move");
#endif
    }
    enum WalkState{Idle, Forward, Back, Left, Right}

    [Header("Move")]
    public float moveSpeed = 1f;
    public InputActionReference leftHandJoystick;

    [Header("Rotation")]
    [SerializeField]
    Transform centerEye;
    [SerializeField]
    private float rotateStartThreshold = 30;
    [SerializeField]
    private float rotateFinishThreshold = 20;
    [SerializeField]
    private float timeToReachMaxRotSpeed = 1;
    [SerializeField]
    private float maxRotationSpeed = 15;
    
    [Space, Range(0, 2)]
    public float timeToReachMinRotationSpeed = 1;
    [SerializeField, Tooltip("If angle between hmd and robot is this value, the rotation speed becomes max")]
    public float angleToReachMaxRotationSpeed = 90;

    private Transform tr;
    private Quaternion targetRot;
    private Animator anim;
    private Rigidbody rb;
    Vector3 moveDir = Vector3.zero;
    private float deltaTime;
    WalkState walkState;

    
    float angle, absAngle;
    float rotSpeed, startRotSpeed;

    WalkState walkStateProperty
    {
        get { return walkState; }
        set 
        {
            if (walkState == value) return;

            walkState = value;
            photonView.CustomRPC(this, "CrossFade", RpcTarget.All, walkState, anim.GetBool("Rotating"));
        }
    }

    [PunRPC]
    private void CrossFade(WalkState state, bool rotating)
    {
        if (state == WalkState.Idle)
            anim.CrossFade(rotating ? "Rotate" : "Idle", 0.2f);
        else
            anim.CrossFade(state.ToString(), 0.2f);
    }

    void Awake()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (photonView.cachedMine)
        // StartCoroutine(IEStartRotate());
        StartCoroutine(IERotateVer2());
    }

    private void FixedUpdate() {
        if (photonView.cachedMine)
        rb.AddForce(moveDir, ForceMode.VelocityChange);
    } 


    // Update is called once per frame
    void Update()
    {
        if (photonView.cachedMine == false)
            return;
        deltaTime = Time.deltaTime;
        UpdateMove();
    }

    private void UpdateMove()
    {
        moveDir = Vector3.zero;
        Vector2 inputDir = leftHandJoystick.action.ReadValue<Vector2>();

        float absX = Mathf.Abs(inputDir.x);
        float absY = Mathf.Abs(inputDir.y);

        // walk
        if (absX > 0.5f || absY > 0.5f)
        {
            // move left or right
            if (absX > absY)
            {
                int round = Mathf.RoundToInt(inputDir.x);
                moveDir = tr.right * round;

                walkStateProperty = round == 1 ? WalkState.Right : WalkState.Left;
            }
            //move forward or backward
            else
            {
                int round = Mathf.RoundToInt(inputDir.y);
                moveDir = tr.forward * round;

                walkStateProperty = round == 1 ? WalkState.Forward : WalkState.Back;
            }
        }
        // not walk
        else
            walkStateProperty = WalkState.Idle;
    }

    // IEnumerator IEStartRotate()
    // {
    //     float angle = 0;

    //     while(true)
    //     {
    //         Vector3 projectedCenterEyeFwdDir = ;
    //         angle = Vector3.SignedAngle(Vector3.ProjectOnPlane(centerEye.forward, tr.up), tr.forward, tr.up);
    //         if (Mathf.Abs(angle) > rotateStartThreshold)
    //         {
    //             // Increase Rotation Speed
    //             for (float t = 0; t < 1; t += deltaTime / timeToReachMaxRotSpeed)
    //             {
    //                 projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
    //                 angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);
    //                 targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * angle);

    //                 tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * maxRotationSpeed * t);
    //                 yield return null;
    //             }

    //             // Rotate with fixed speed
    //             while (Mathf.Abs(angle) > rotateFinishThreshold)
    //             {
    //                 projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
    //                 angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);

    //                 targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * angle);
    //                 tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * maxRotationSpeed);
    //                 yield return null;
    //             }

    //             // Decrease Rotation Speed
    //             for (float t = 1; t > 0; t -= deltaTime / timeToReachMaxRotSpeed)
    //             {
    //                 projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
    //                 angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);

    //                 targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * angle);

    //                 tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * maxRotationSpeed * t);
    //                 yield return null;
    //             }
    //         }

    //         yield return null;
    //     }
    // }
    // 각도에 따른 회전 속도
    IEnumerator IERotateVer2()
    {
        startRotSpeed = maxRotationSpeed * (rotateStartThreshold/angleToReachMaxRotationSpeed);

        while(true)
        {
            CalculateAngle();

            // Start rotate when angle between robot's forward direction and centereye's forward direction larger than threshold
            if (absAngle > rotateStartThreshold)
            {
                anim.SetBool("Rotating", true);
                anim.SetBool("RotateMirror", angle > 0 ? false : true);
                rotSpeed = 0;
                while (absAngle > rotateFinishThreshold)
                {
                    CalculateAngle();
                    rotSpeed = rotSpeed < startRotSpeed ? Mathf.Lerp(rotSpeed, startRotSpeed+1, deltaTime / timeToReachMinRotationSpeed) : maxRotationSpeed * Mathf.Clamp01((absAngle/angleToReachMaxRotationSpeed));
                    
                    Rotate();
                    yield return null;
                }

                for (float f = 0; f < 1; f += deltaTime)
                {
                    rotSpeed = Mathf.Lerp(rotSpeed, 0, f);
                    Rotate();
                    yield return null;
                }
                anim.SetBool("Rotating", false);
            }

            yield return null;
        }
    }

    void CalculateAngle()
    {
        angle = Vector3.SignedAngle(Vector3.ProjectOnPlane(centerEye.forward, tr.up), tr.forward, tr.up);
        absAngle = Mathf.Abs(angle);
    }
    void Rotate()
    {
        if (walkState != WalkState.Idle)
        {
            targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * angle);
            tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * rotSpeed);
        }
        else
        {
            anim.SetFloat("TurnSpeed", rotSpeed/45);
        }
    }

    public void SetMoveSpeed(float _value) => moveSpeed = _value;
    public void SetRotSpeed(float _value) => maxRotationSpeed = _value;
}
