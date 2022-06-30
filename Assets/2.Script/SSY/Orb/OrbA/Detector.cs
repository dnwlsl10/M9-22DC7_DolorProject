using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Detector : MonoBehaviour
{
    struct study // 스트럭트 - 값을 저장하는 컨테이너와 같은 / 
    {
        public Connector connector;
        public WeaponSystem weaponSystem;
    }
    // 딕셔너리 - 1대1 매칭 때 사용하기 용이, 키값은 유니크해야함!!!!!(단하나와 같은)
    public GameObject linkPrefab;
    Dictionary<Transform, study> dic = new Dictionary<Transform, study>();

    void OnTriggerEnter(Collider other) //OrbA에 맞았을 때 맞은 대상 == UI에 방패불가라는 텍스트를 띄어주는
    {
        if (other.transform.root.tag.Equals("Enemy")) //내가 아닌 나 == 상대방
        {
            if (dic.ContainsKey(other.transform.root)) return;

            print("FIND ENEMY");

            study s = new study();

            GameObject link = Instantiate(linkPrefab);
            link.transform.parent = this.transform;
            link.transform.localPosition = Vector3.zero;
            s.connector = link.GetComponent<Connector>();
            s.connector.SetTarget(other.transform.root);


            WeaponSystem weaponSystem = other.transform.root.GetComponentInChildren<WeaponSystem>();
            weaponSystem.canUseSkill[(int)WeaponName.Shield] = false;
            weaponSystem.StopWeaponEvent(WeaponName.Shield);

            s.weaponSystem = weaponSystem;
            dic.Add(other.transform.root, s);
        }
    }

    void OnTriggerExit(Collider other) // 방패불가라는 텍스트가 꺼짐.
    {
        if (dic.TryGetValue(other.transform.root, out study s))
        {
            print("Exited");
            s.weaponSystem.canUseSkill[(int)WeaponName.Shield] = true;

            Destroy(s.connector.gameObject);
        }
    }
}
