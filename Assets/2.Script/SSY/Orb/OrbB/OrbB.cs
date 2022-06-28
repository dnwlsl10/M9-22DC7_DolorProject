using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Orb의 기능
// 범위폭발, 시야방해(데미지, 속도, 받는 데미지)

// 생성한 "OrbB"가 내 발사체의 테이크 데미지를 받으면 "OrbB"에 발사속도의 가속도가 붙어(제이스 E-Q) 재빠르게 진행방향으로 날아간다.
// "OrbB"가 어딘가에 부딪히면 OrbB오브젝트의 OrbVFX가 SetAcitive(false)되고, "BlackHole"오브젝트에 스케일이 커지는 애니메이션 실행(Play)되면서
// 범위폭발을 일으켜 범위에 맞은 적은 데미지를 입고 시야를 방해받게 된다.
public class OrbB : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TakeDamage() 
    {

    }
    void Damage()
    {

    }
    void OnCollisionEnter(Collision other) 
    {
        
    }
}
