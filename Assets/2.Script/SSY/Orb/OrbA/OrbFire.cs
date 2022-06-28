using System.Collections;
using UnityEngine;

//오브의 스킬 쿨타임이 상승하여 사용 가능하면
public class OrbFire : WeaponBase
{
    public event Cur_MaxEvent OnValueChange;
    public GameObject firePosition;
    public GameObject orbFactory;
    GameObject orb;

    public float currentSkillCT;//현재 스킬 쿨타임
    public float maxSkillCT = 5;//최대 스킬 쿨타임

    void Start()
    {
        currentSkillCT = 0; //게임 시작 시 스킬은 0으로 초기화   
    }
    void Update()
    {
        currentSkillCT += Time.deltaTime;
        if (currentSkillCT >= maxSkillCT)
        {
            currentSkillCT = maxSkillCT;
        }
    }

    public UnityEngine.InputSystem.InputActionReference alpha1;

    private void OnEnable()
    {
        alpha1.action.started += StartEvent;
        alpha1.action.canceled += StopEvent;
    }
    private void OnDisable()
    {
        alpha1.action.started -= StartEvent;
        alpha1.action.canceled -= StopEvent;
    }

    void StartEvent(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        StartWeaponAction();
    }
    void StopEvent(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        StopWeaponAction();
    }
    public override void StartWeaponAction() //GetKeyDown
    {
        if (Time.time - lastAttackTime < weaponSetting.attackRate) //만약 스킬 쿨타임 중이면 스킬 사용할 수 없다는 소리가 나면서 사용불가
            return;
        orb = Instantiate(orbFactory);
        orb.transform.SetParent(firePosition.transform);
        orb.transform.position = firePosition.transform.position;
        orb.transform.rotation = firePosition.transform.rotation;
    }
    public override void StopWeaponAction() //GetKeyUp //오브 발사하고
    {
        lastAttackTime = Time.time; //초기화
        OrbMove orbMove = orb.GetComponent<OrbMove>();
        orbMove.OrbSpeedChange();
    }

}

