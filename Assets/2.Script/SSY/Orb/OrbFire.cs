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

    public override void StartWeaponAction() //GetKeyDown
    {
        print("Start Orb");
        if (Time.time - lastAttackTime < weaponSetting.attackRate) //만약 스킬 쿨타임 중이면 스킬 사용할 수 없다는 소리가 나면서 사용불가
            return;

        StartCoroutine(Hold());
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

        lastAttackTime = Time.time; //초기화
        OrbBase orbMove = orb.GetComponent<OrbBase>();
        orbMove.OrbFire();
        orb = null;
    }
}

