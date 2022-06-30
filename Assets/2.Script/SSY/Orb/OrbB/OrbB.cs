using System.Collections;
using UnityEngine;

//Orb의 기능
// 범위폭발, 시야방해(데미지, 속도, 받는 데미지)

// 생성한 "OrbB"가 내 발사체의 테이크 데미지를 받으면 "OrbB"에 발사속도의 가속도가 붙어(제이스 E-Q) 재빠르게 진행방향으로 날아간다.
// "OrbB"가 어딘가에 부딪히면 OrbB오브젝트의 OrbVFX가 SetAcitive(false)되고, "BlackHole"오브젝트에 스케일이 커지는 애니메이션 실행(Play)되면서
// 범위폭발을 일으켜 범위에 맞은 적은 데미지를 입고 시야를 방해받게 된다.
public class OrbB : OrbBase
{
    public int count = 0;
    public float startSize = 3;
    
    void Start()
    {
        transform.GetChild(1).localScale = Vector3.one*startSize; //ORB의 자식 2번째 오브젝트 스케일을 3으로 지정.
        count = 5;
    }
    protected override void Init() //등장하는 순간
    {
        base.Init();
    
        gameObject.GetComponent<SphereCollider>().enabled = true; //콜라이더 끄기
        transform.GetChild(1).localScale = Vector3.one * startSize; //스케일 다시 초기화
        transform.GetChild(0).gameObject.SetActive(true); //오브 VFX도 다시 켜주고
        transform.GetChild(1).gameObject.SetActive(true);

    }

    void OnTriggerEnter(Collider other) //무조건 트리거여야한다 - 콜리전이면 나중에 물리법칙을 받게 될 수 있따. 
    //나의 총알이 닿았을 때와  // 어떠한 충돌체와 닿았을 때
    {
        print("!!!");
        if (other.gameObject.layer == LayerMask.NameToLayer("RemotePlayer") || other.gameObject.layer == LayerMask.NameToLayer("Builder")) //충돌체
        {
        //     bool robotDamaged;
        //     Physics.OverlapSphere(transform.position,10f);//로봇이든 건물이든 transforkm.root. 특정. 하나에만
        //    // for 오버랩스피어 콜라이더 받은거 하나씩 비교
        //    for(int i = 0; i )
        //     if (true) // 콜라이더가 로봇거면
        //         if (robotDamaged == false)
        //         {

        //         } //로봇이 데미지를 입은적이 없다면
        //             // 데미지 준다
        //             // + robotDamaged = true;
        //         else // 로봇이 데미지를 입은적이 있다.
            StartCoroutine("OrbBomb");
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bullet")) // 총알이면
        {
            if (count == 0)
            {
                return; // 부딪힌 총알 파괴하지 않겠
            }

            count--;
            orbSpeed++;
            //scale 줄이기
            transform.GetChild(1).localScale -= Vector3.one * 0.5f; //스케일 다시 초기화
            //총알.getcomponent().SelfDestory();
        }
    }
    IEnumerator OrbBomb() //터질 때
    {
        transform.GetChild(0).gameObject.SetActive(false); // 자식 오브젝트 첫번째를 부딪힌 순간 꺼주고

        // 스케일 키우고
        while(transform.GetChild(1).localScale.x < 10) //사이즈가 10 전까지 반복.
        {
            transform.GetChild(1).localScale += Vector3.one * Time.deltaTime;
            yield return null; 
        }
        yield return new WaitForSeconds(1.5f);
        // 몇초뒤
        while (transform.GetChild(1).localScale.x > 0.1f) //사이즈가 0.1 전까지 반복.
        {
            transform.GetChild(1).localScale -= Vector3.one * Time.deltaTime *30;
            yield return null;
        }
        transform.GetChild(1).gameObject.SetActive(false);
        //줄어들고 꺼지기
        
    }

    public override void OrbFire() //발사한 순간
    {
        base.OrbFire();
        //콜라이더 켜기
        gameObject.GetComponent<SphereCollider>().enabled = false; //콜라이더 끄기

    }
}

