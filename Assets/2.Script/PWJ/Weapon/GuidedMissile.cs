using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GuidedMissile : WeaponBase , IInitialize
{
    public event Cur_MaxEvent OnValueChange;

    [Header("CrossHair")]
    [SerializeField]
    private GuidedMissileCrossHair gmSystem;

    [Header("SpawnPoint")]
    public Transform bulletSpawnPoint;
    [Header("CurvedRangePath")]
    public List<Transform> randomPath;

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;
    [SerializeField]
    private GameObject bullet;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip onFireSFX;
    [SerializeField]
    private AudioClip onReloadSFX;
    private WaitForEndOfFrame eof = new WaitForEndOfFrame();
    private WaitForSeconds ar = new WaitForSeconds(0.2f);
    public System.Action OnPress;
    public System.Action OnCancle;

    public bool isAutomatic;
    Coroutine coroutineHolder;

    [Header("Remote Player")]
    [SerializeField]
    private Transform target;

    [ContextMenu("Init")]
    public void Reset()
    {
#if UNITY_EDITOR
        weaponSetting.weaponName = WeaponName.Missile;
        weaponSetting.maxAmmo = 20;
        weaponSetting.attackDistance = 0;
        weaponSetting.attackRate = 0.2f;
        weaponSetting.damage = 1;
        handSide = HandSide.Right;
        isAutomatic = true;
        if(gmSystem == null)
            gmSystem = this.GetComponent<GuidedMissileCrossHair>();
        if (bullet == null)
            bullet = Resources.Load("MissileProjectile") as GameObject;

        if (bulletSpawnPoint == null)
        {
            Utility.GetBoneTransform(transform.root, HumanBodyBones.Neck, out Transform forearm);
            bulletSpawnPoint = Utility.FindChildContainsName(forearm, new string[] { "GuidedMissile", "guidedmissile" });
            if (bulletSpawnPoint == null)
            {
                bulletSpawnPoint = new GameObject("GuidedMissile").transform;
                bulletSpawnPoint.parent = forearm;
                bulletSpawnPoint.localPosition = new Vector3(0, -0.001199978f, -0.01820103f);
                bulletSpawnPoint.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        else{
        
            for(int i =0; i < bulletSpawnPoint.childCount; i++){
                randomPath.Add(bulletSpawnPoint.GetChild(i));
            }
        }
#endif
    }


    public float CurrentAmmo
    {
        get { return weaponSetting.currentAmmo; }
        set
        {
            float prevAmmo = weaponSetting.currentAmmo;
            weaponSetting.currentAmmo = Mathf.Clamp(value, 0, weaponSetting.maxAmmo);
           
            if (prevAmmo != weaponSetting.currentAmmo)
            {
                OnValueChange?.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
            }
        }
    }

    public override void Initialize()
    {
        CurrentAmmo = 0;
        OnValueChange?.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
        if (SceneManager.GetActiveScene().name == "Connect") return;

        StartReload();
    }
    public override void StartWeaponAction() //키를 누르때
    {   
        if(CurrentAmmo < weaponSetting.maxAmmo || bLock) return;

        if(gmSystem.state == eState.Normal)
        {
            gmSystem.state = eState.Tracking;
            gmSystem.StartGuidedMissile();
            OnPress();
        }
    }


    public override void StopWeaponAction() //키를 땠을때 
    {
        OnCancle();
        if(gmSystem.state == eState.Tracking)
        {
            gmSystem.StopGuidedMissile();
            gmSystem.state = eState.Normal;
        }

        if(bFire || CurrentAmmo < weaponSetting.maxAmmo) return;
        
        if(gmSystem.state == eState.TrackingComplete)
        {
            coroutineHolder = StartCoroutine(ContinuousFire());
            bFire = true;
        } 
    }

    public override void StartReload()
    {
        if(isReloading) return;

        StartCoroutine(OnReload());
    }
    IEnumerator ContinuousFire()
    {
        while (CurrentAmmo > 0)
        {
            yield return ar;
            OnAttack();
        }
    }

    private void OnAttack()
    {
        this.target = gmSystem.enemyTarget;

        int randIndex = Random.Range(0, randomPath.Count - 1);
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(0.1f, 1f), 0);

        dir.Normalize();
        dir += new Vector3(0, 0, -8.25f);

        Vector3 p1 = bulletSpawnPoint.transform.position;
        Vector3 p2 = new Vector3(randomPath[randIndex].position.x, randomPath[randIndex].position.y, 0) + dir;
        Vector3 p3 = target.position;

        var missile = NetworkObjectPool.instance.SpawnFromPool<Missile>(bullet.name, bulletSpawnPoint.transform.position, Quaternion.identity);
        missile.gm = this;


        photonView.CustomRPC(missile, "RPCPath", RpcTarget.AllViaServer, p1, p2, p3);

        StartCoroutine(OnMuzzleFlashEffect());
        PlaySound(onFireSFX);
        CurrentAmmo--;
    }

    #region  DestoryCount
    int mcount;
    bool bFire;
    int destoryCount = 0;
    public void Destory(){
        destoryCount++;
        Debug.Log("미사일 파괴 갯수");
        if(destoryCount == weaponSetting.maxAmmo){
            gmSystem.StopGuidedMissile();
            StartReload();
        }
    }
#endregion

    IEnumerator OnMuzzleFlashEffect()
    {
        if (muzzleFlashEffect == null) yield break;

        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.6f);
        muzzleFlashEffect.SetActive(false);
    }

    IEnumerator OnReload()
    {
        isReloading = true;

        yield return new WaitForSeconds(1f);
        while (weaponSetting.currentAmmo < weaponSetting.maxAmmo)
        {
            yield return null;
            CurrentAmmo += 10f * Time.deltaTime; 
        }
        OnDefult();
    }

    private void OnDefult()
    {
        CurrentAmmo = weaponSetting.maxAmmo;
        isReloading = false;
        bFire = false;
        mcount = 0;
        destoryCount = 0;
        gmSystem.state = eState.Normal;
    }
}
