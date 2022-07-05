using System.Collections;
using UnityEngine;
using Photon.Pun;

//오브의 스킬 쿨타임이 상승하여 사용 가능하면
public class OrbFire : WeaponBase
{
    public event Cur_MaxEvent OnValueChange;
     public Transform firePosition;
    public GameObject[] orbFactory;
    GameObject orb;
    public int orbType;
    public float cooldown;
    public bool bUseOrb;
    public System.Action OnPress;
    public System.Action OnCancle;

    public float Cooldown{
        get { return cooldown;}
        set
        {
            float preVal = cooldown;
            cooldown = Mathf.Clamp(value, 0, weaponSetting.attackRate);
            OnValueChange?.Invoke(cooldown, weaponSetting.attackRate);
        }
    }

    public override void Initialize(){
        base.Initialize();
        cooldown = weaponSetting.attackRate;
    }

    public override void StartWeaponAction() //GetKeyDown
    {
        print("Start Orb"); 
        if (Time.time - lastAttackTime < weaponSetting.attackRate) //만약 스킬 쿨타임 중이면 스킬 사용할 수 없다는 소리가 나면서 사용불가
            return;
        StartCoroutine(Hold());
        OnPress();
    }

    IEnumerator Hold()
    {
        orb = NetworkObjectPool.instance.SpawnFromPool(orbFactory[orbType].name, firePosition.position, firePosition.rotation);
        yield return new WaitForEndOfFrame();
        orb.GetComponent<OrbBase>().SetParent(firePosition);

        yield return new WaitForSecondsRealtime(4f);
        yield return new WaitForEndOfFrame();
        
        StopWeaponAction();
    }

    public override void StopWeaponAction() //GetKeyUp //오브 발사하고
    {
        if (orb == null)
            return;

        OnCancle();
        lastAttackTime = Time.time; //초기화
        StartCoroutine(StartCooldown());
        OrbBase orbMove = orb.GetComponent<OrbBase>();
        orbMove.OrbFire();
        orb = null;
    }

    IEnumerator StartCooldown(){
        
        yield return null;

        while(Cooldown > 0)
        {
            yield return new WaitForSeconds(1f);
            Cooldown -= 1;
        }
        Cooldown = weaponSetting.attackRate;
    }
}

