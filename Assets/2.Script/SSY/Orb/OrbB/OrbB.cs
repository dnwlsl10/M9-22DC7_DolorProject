using System.Collections;
using UnityEngine;
using Photon.Pun;
public class OrbB : OrbBase
{
    [SerializeField] private float speedUp;

    [Header("Scale")]
    [SerializeField] private float startSize = 3;
    [SerializeField] float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float timeToMaxScale;
    private float scaleDown;

    [Space]
    [SerializeField] int maxCount;
    int count = 0;
    [SerializeField] LayerMask ChoongDolChae;
    SphereCollider sphereCollider;

    void Awake() 
    {
        sphereCollider = GetComponentInChildren<SphereCollider>();
        scaleDown = (startSize - minScale)/maxCount;

        if (photonView.Mine == false) sphereCollider.enabled = false;
    }
    protected override void Init() //등장하는 순간
    {
        base.Init();

        if (photonView.Mine) sphereCollider.enabled = true; //콜라이더 끄기 //나중에 false 로 변경

        transform.GetChild(1).localScale = Vector3.one * startSize; //스케일 다시 초기화
        transform.GetChild(0).gameObject.SetActive(true); //오브 VFX도 다시 켜주고
        transform.GetChild(1).gameObject.SetActive(true);

        count = 0;
    }
    
    IEnumerator coroutine;

    // 오브를 쏜 사람만 트리거 감지 가능. why ? Awake에서 다른사람은 콜라이더를 껐음
    void OnTriggerEnter(Collider other) //무조건 트리거여야한다 - 콜리전이면 나중에 물리법칙을 받게 될 수 있따. 
    //나의 총알이 닿았을 때와  // 어떠한 충돌체와 닿았을 때
    {
        print("!!!");
        if (ChoongDolChae == (ChoongDolChae | (1 << other.gameObject.layer))) //충돌체
        {
            sphereCollider.enabled = false;
            bool robotDamaged = false;

            Collider[] cols = Physics.OverlapSphere(transform.position, maxScale, ChoongDolChae);
            int remotePlayerLayer = LayerMask.NameToLayer("RemotePlayer");
            foreach (var collider in cols)
            {
                if (collider.gameObject.layer == remotePlayerLayer)
                {
                    if (robotDamaged == true)
                    {
                        continue;
                    }
                    else
                    {
                        robotDamaged = true;
                        if (collider.transform.root.TryGetComponent<PhotonView>(out PhotonView pv) && pv.ViewID > 0)
                        {
                            photonView.CustomRPC(this, "CDCHit", RpcTarget.All, pv.ViewID, transform.position);
                        }
                        else // 싱글모드 일때
                        {
                            photonView.CustomRPC(this, "CDCHit", RpcTarget.All, 0, transform.position);
                            if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
                                damageable.TakeDamage(100);
                        }
                    }
                }
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Bullet")) // 총알이면
        {
            print(count);
            if (count == maxCount)
            {
                return;
            }

            count++;
            photonView.CustomRPC(this, "BulletHit", RpcTarget.All);

            if (other.gameObject.TryGetComponent<ProjectileBase>(out var projectileBase))
            {
                // projectileBase.SelfDestroy();
            }
        }
    }
    
    [PunRPC]
    void CDCHit(int viewID, Vector3 intersection)
    {
        transform.position = intersection;
        orbSpeed = 0; // 네트워크 공유
        StartCoroutine("OrbBomb"); // 네트워크 공유

        if (viewID != 0 && PhotonNetwork.GetPhotonView(viewID).TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(100);
        }
    }

    [PunRPC]
    void BulletHit()
    {
        if (coroutine != null) // 네트워크 공유
        {
            StopCoroutine(coroutine); // 네트워크 공유
        }
        // 코루틴이 이미 돌고있다
        coroutine = asdnansndlksan(onShootSpeed + count * speedUp, startSize - count * scaleDown); // 네트워크 공유
        StartCoroutine(coroutine); // 네트워크 공유
    }

    IEnumerator asdnansndlksan(float targetSpeed, float targetScale) //총알을 한번 맞을때
    {
        Transform child = transform.GetChild(1);
        for (float f = 0; f < 0.5f; f += Time.deltaTime) // 0.1f == 지금 총알과 다음 총알 딜레이 시간 만큼 하면 자연스러울 것.!!!
        {
            orbSpeed = Mathf.Lerp(orbSpeed, targetSpeed, f/0.1f);
            child.localScale = Mathf.Lerp(child.localScale.x, targetScale, f/0.1f) * Vector3.one;
            yield return null;
        }

        orbSpeed = targetSpeed;
        child.localScale = Vector3.one * targetScale;
    }
    
    IEnumerator OrbBomb() //터질 때
    {
        transform.GetChild(0).gameObject.SetActive(false); // 자식 오브젝트 첫번째를 부딪힌 순간 꺼주고

        Transform child = transform.GetChild(1);
        for (float f = 0; f < timeToMaxScale; f += Time.deltaTime)
        {
            child.localScale = Vector3.one * Mathf.Lerp(child.localScale.x, maxScale, f / timeToMaxScale);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        for (float f = 0; f < timeToMaxScale; f += Time.deltaTime)
        {
            child.localScale = Vector3.one * Mathf.Lerp(child.localScale.x, 0, f / timeToMaxScale);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    protected override void RPCFire(Vector3 shootPosition, Vector3 forward) //발사한 순간
    {
        base.RPCFire(shootPosition, forward);
        //콜라이더 켜기
        if (photonView.Mine)
            sphereCollider.enabled = true;
    }
}

