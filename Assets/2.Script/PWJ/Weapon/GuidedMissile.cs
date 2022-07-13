using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GuidedMissile : WeaponBase , IInitialize
{
    [Header("CrossHair")]
    [SerializeField]
    private GuidedMissileCrossHair gmSystem;

    [Header("SpawnPoint")]
    public Transform bulletSpawnPoint;

    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;
    [SerializeField]
    private GameObject bullet;

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip onFireSFX;
    [SerializeField]
    private AudioClip onFailedSFX;
    private WaitForEndOfFrame eof = new WaitForEndOfFrame();
    private WaitForSeconds ar = new WaitForSeconds(0.2f);

    private float upGaugeRate;
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
        weaponSetting.damage = 10;
        weaponSetting.bLock = false;
 
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
                ValueChangeEvent(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        CurrentAmmo = 0;
        upGaugeRate = 10f;
        if(SceneManager.GetActiveScene().name == "Connect") return;
        StartReload();
    }
    public override void StartWeaponAction() //키를 누르때
    {   
        Debug.Log("Missle Test");
        if(CurrentAmmo < weaponSetting.maxAmmo) return;

        if(gmSystem.state == eState.Normal)
        {
            gmSystem.state = eState.Tracking;
            gmSystem.StartGuidedMissile();
            WeaponSystem.instance.StartActionCallback((int)weaponSetting.weaponName);
        }
    }


    public override void StopWeaponAction() //키를 땠을때 
    {
        if (bFire || CurrentAmmo < weaponSetting.maxAmmo) return;
        WeaponSystem.instance.StopActionCallback((int)weaponSetting.weaponName);
        if(gmSystem.state == eState.Tracking)
        {
            gmSystem.StopGuidedMissile();
            gmSystem.StopOnTracking();
            gmSystem.state = eState.Normal;
            AudioPool.instance.Play(onFailedSFX.name, 2, this.transform.position);
        }

        if(gmSystem.state == eState.TrackingComplete)
        {
            bFire = true;
            gmSystem.StopGuidedMissile();
            gmSystem.state = eState.Fire;
           StartCoroutine(LaunchMissile());
        } 
    }

    public override void StartReload()
    {
        if(isReloading) return;

        if(weaponSetting.bLock) upGaugeRate = 10f;
        StartCoroutine(OnReload(upGaugeRate));
    }

    PhotonView targetPV;
    IEnumerator LaunchMissile()
    {
        this.target = gmSystem.enemyTarget;
        targetPV = target.GetComponent<PhotonView>();
        lastAttackTime = 0;
        while (CurrentAmmo > 0)
        {
            yield return new WaitForEndOfFrame();
            if (lastAttackTime < weaponSetting.attackRate)
            {
                lastAttackTime += Time.deltaTime;
            }
            else
            {
                lastAttackTime = 0;
                OnAttack();
            }
        }
    }

    public void GetGauge(){
        CurrentAmmo += 0.05f;
    }
    private void OnAttack()
    {
        Quaternion qrot = Quaternion.Euler(new Vector3(Random.Range(0f, -90f), Random.Range(90f, 270f),0));
        var missile = NetworkObjectPool.instance.SpawnFromPool<Missile>(bullet.name, bulletSpawnPoint.transform.position, qrot, false);
        missile.gm = this;
        missile.damage  = weaponSetting.damage;
        AudioPool.instance.Play(onFireSFX.name, 2, bulletSpawnPoint.position);
        
        if(targetPV?.ViewID > 0)
            missile.photonView.RPC("SetTargetRPC", RpcTarget.AllViaServer, targetPV.ViewID);
        else
        {
            print("AAA");
            missile.Launch(target);
        }

        StartCoroutine(OnMuzzleFlashEffect());
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
            bFire = false;
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

    IEnumerator OnReload(float upGaugeVal)
    {
        isReloading = true;

        yield return new WaitForSeconds(1f);
        while (weaponSetting.currentAmmo < weaponSetting.maxAmmo)
        {
            yield return null;
            CurrentAmmo += upGaugeVal * Time.deltaTime; 
        }
        OnDefult();
    }

    private void OnDefult()
    {
        CurrentAmmo = weaponSetting.maxAmmo;
        isReloading = false;
        mcount = 0;
        destoryCount = 0;
        gmSystem.state = eState.Normal;
    }
}
