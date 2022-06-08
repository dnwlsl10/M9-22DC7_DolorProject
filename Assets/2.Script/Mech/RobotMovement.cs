using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class RobotMovement : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 1f;
    public InputActionProperty rightHandJoystick;
    public InputActionProperty leftHandJoystick;

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
    [SerializeField, Tooltip("If angle between hmd and robot is this value, the rotation speed becomes max")]
    public float angleToReachMaxRotationSpeed = 90;

    private Transform tr;
    private Quaternion targetRot;
    private Animator anim;
    private float deltaTime;

    void Awake()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // StartCoroutine(IEStartRotate());
        StartCoroutine(IERotateVer2());
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.deltaTime;

        UpdateMove();
    }

    private void UpdateMove()
    {
        bool walk = true;

        Vector2 inputDir = rightHandJoystick.action.ReadValue<Vector2>();
        if (inputDir == Vector2.zero)
            inputDir = leftHandJoystick.action.ReadValue<Vector2>();

        anim.SetFloat("moveX", inputDir.x);
        anim.SetFloat("moveY", inputDir.y);

        if (inputDir.y > 0.5f)
        {
            tr.position += tr.forward * moveSpeed * deltaTime;
            anim.SetBool("Walk", true);
        }
        else if (inputDir.y < -0.5f)
        {
            tr.position -= tr.forward * moveSpeed * deltaTime;
            anim.SetBool("Walk", true);
        }
        else
        {
            walk = false;
        }
        
        if (inputDir.x > 0.5f)
        {
            tr.position += tr.right * moveSpeed * deltaTime;
            anim.SetBool("Walk", true);
        }
        else if (inputDir.x < -0.5f)
        {
            tr.position -= tr.right * moveSpeed * deltaTime;
            anim.SetBool("Walk", true);
        }
        else if (walk == false)
        {
            anim.SetBool("Walk", false);
        }
    }

    IEnumerator IEStartRotate()
    {
        float angle = 0;

        while(true)
        {
            Vector3 projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
            angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);
            if (Mathf.Abs(angle) > rotateStartThreshold)
            {
                // Increase Rotation Speed
                for (float t = 0; t < 1; t += deltaTime / timeToReachMaxRotSpeed)
                {
                    projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
                    angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);
                    targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * angle);

                    tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * maxRotationSpeed * t);
                    yield return null;
                }

                // Rotate with fixed speed
                while (Mathf.Abs(angle) > rotateFinishThreshold)
                {
                    projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
                    angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);

                    targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * angle);
                    tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * maxRotationSpeed);
                    yield return null;
                }

                // Decrease Rotation Speed
                for (float t = 1; t > 0; t -= deltaTime / timeToReachMaxRotSpeed)
                {
                    projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
                    angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);

                    targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * angle);

                    tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * maxRotationSpeed * t);
                    yield return null;
                }
            }

            yield return null;
        }
    }

    // 각도에 따른 회전 속도
    IEnumerator IERotateVer2()
    {
        float angle = 0;
        float absAngle = 0;
        float rotSpeed = 0;
        float t = 0;
        float startRotSpeed = maxRotationSpeed * (rotateStartThreshold/angleToReachMaxRotationSpeed);

        while(true)
        {
            Vector3 projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
            angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);
            absAngle = Mathf.Abs(angle);

            // Start rotate when angle between robot's forward direction and centereye's forward direction larger than threshold
            if (absAngle > rotateStartThreshold)
            {
                anim.SetBool("Rotating", true);
                rotSpeed = 0;
                t = 0;
                while (absAngle > rotateFinishThreshold)
                {
                    projectedCenterEyeFwdDir = Vector3.ProjectOnPlane(centerEye.forward, tr.up);
                    angle = Vector3.SignedAngle(projectedCenterEyeFwdDir, tr.forward, tr.up);
                    absAngle = Mathf.Abs(angle);

                    if (rotSpeed < startRotSpeed)
                    {
                        t += deltaTime;
                        rotSpeed = Mathf.Lerp(rotSpeed, startRotSpeed, t);
                    }
                    else
                    {
                        rotSpeed = maxRotationSpeed * Mathf.Clamp((absAngle/angleToReachMaxRotationSpeed), 0, 1);
                    }

                    if (anim.GetBool("Walk") == true)
                    {
                        targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * angle);
                        tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * rotSpeed);
                    }
                    else
                    {
                        anim.SetFloat("TurnSpeed", rotSpeed/45);
                        anim.SetBool("RotateMirror", angle > 0 ? false : true);
                    }
                    yield return null;
                }

                anim.SetBool("Rotating", false);

                if (anim.GetBool("Walk"))
                {
                    for (float f = 0; f < 1; f += deltaTime)
                    {
                        rotSpeed = Mathf.Lerp(rotSpeed, 0, f);
                        targetRot = Quaternion.Euler(tr.eulerAngles - tr.up * (angle > 0 ? 1 : -1) * rotSpeed);
                        tr.rotation = Quaternion.RotateTowards(tr.rotation, targetRot, deltaTime * rotSpeed);
                    }
                }
            }

            yield return null;
        }
    }

    public void SetMoveSpeed(float _value) => moveSpeed = _value;
    public void SetRotSpeed(float _value) => maxRotationSpeed = _value;
}
