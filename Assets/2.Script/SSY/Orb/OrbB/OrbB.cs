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
    [SerializeField] LayerMask explosionLayer;
    SphereCollider sphereCollider;

    [SerializeField] Transform vfx;
    [SerializeField] Transform blackHole;
    bool robotDamaged;

    void Awake() 
    {
        sphereCollider = GetComponentInChildren<SphereCollider>();
        scaleDown = (startSize - minScale)/maxCount;
    }
    protected override void Init() //등장하는 순간
    {
        base.Init();

        if (photonView.Mine) sphereCollider.enabled = false;

        blackHole.localScale = Vector3.one * startSize; //스케일 다시 초기화
        blackHole.gameObject.SetActive(true);
        vfx.gameObject.SetActive(true); //오브 VFX도 다시 켜주고

        count = 0;
        robotDamaged = false;
    }
    
    IEnumerator coroutine;

    // 오브를 쏜 사람만 트리거 감지 가능. why ? Awake에서 다른사람은 콜라이더를 껐음
    void OnTriggerEnter(Collider other) //무조건 트리거여야한다 - 콜리전이면 나중에 물리법칙을 받게 될 수 있따. 
    //나의 총알이 닿았을 때와  // 어떠한 충돌체와 닿았을 때
    {
        if (explosionLayer == (explosionLayer | (1 << other.gameObject.layer))) //충돌체
        {
            sphereCollider.enabled = false;
            int remoteLayer = LayerMask.NameToLayer("RemoteCapsule");

            Collider[] cols = Physics.OverlapSphere(transform.position, maxScale, explosionLayer);

            foreach (var collider in cols)
                if (collider.gameObject.layer == remoteLayer)
                {
                    if (robotDamaged == false)
                    {
                        robotDamaged = true;
                        CheckOhterHasPV(other.transform);
                    }
                }
        }
        else if (other.tag == "Bullet") // 총알이면
        {
            print("BULLET");
            if (count == maxCount)
                return;
            
            count++;
            var pv = other.gameObject.GetComponent<PhotonView>();
            if (pv?.ViewID > 0 == false)
                other.gameObject.SetActive(false);

            photonView.CustomRPC(this, "BulletHit", RpcTarget.All, pv?.ViewID, count);
        }
    }

    void CheckOhterHasPV(Transform tr)
    {
        var pv = tr.root.GetComponent<PhotonView>();
        if (pv?.ViewID > 0 == false)
            GiveDamage(tr.root, 100);

        photonView.CustomRPC(this, "CDCHit", RpcTarget.All, pv?.ViewID, transform.position);
    }
    
    [PunRPC]
    void CDCHit(int viewID, Vector3 intersection)
    {
        transform.position = intersection;
        orbSpeed = 0; // 네트워크 공유
        StartCoroutine(OrbBomb()); // 네트워크 공유

        if (viewID > 0)
            GiveDamage(PhotonNetwork.GetPhotonView(viewID).transform, 100);
    }

    void GiveDamage(Transform tr, float damage) => tr.GetComponent<IDamageable>()?.TakeDamage(damage);

    [PunRPC]
    void BulletHit(int viewID, int count)
    {
        if (viewID > 0) PhotonNetwork.GetPhotonView(viewID).gameObject.SetActive(false);

        if (coroutine != null) // 네트워크 공유 // 코루틴이 이미 돌고있다
            StopCoroutine(coroutine); // 네트워크 공유
        
        coroutine = CompressSize(onShootSpeed + count * speedUp, startSize - count * scaleDown); // 네트워크 공유
        StartCoroutine(coroutine); // 네트워크 공유
    }

    IEnumerator CompressSize(float targetSpeed, float targetScale) //총알을 한번 맞을때
    {
        print("Compress" + targetScale);
        for (float f = 0; f < 0.5f; f += Time.deltaTime) // 0.1f == 지금 총알과 다음 총알 딜레이 시간 만큼 하면 자연스러울 것.!!!
        {
            orbSpeed = Mathf.Lerp(orbSpeed, targetSpeed, f/0.1f);
            blackHole.localScale = Mathf.Lerp(blackHole.localScale.x, targetScale, f/0.1f) * Vector3.one;
            yield return null;
        }

        orbSpeed = targetSpeed;
        blackHole.localScale = Vector3.one * targetScale;
    }
    
    IEnumerator OrbBomb() //터질 때
    {
        vfx.gameObject.SetActive(false); // 자식 오브젝트 첫번째를 부딪힌 순간 꺼주고

        for (float f = 0; f < timeToMaxScale; f += Time.deltaTime)
        {
            blackHole.localScale = Vector3.one * Mathf.Lerp(blackHole.localScale.x, maxScale, f / timeToMaxScale);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        for (float f = 0; f < timeToMaxScale; f += Time.deltaTime)
        {
            blackHole.localScale = Vector3.one * Mathf.Lerp(blackHole.localScale.x, 0, f / timeToMaxScale);
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

