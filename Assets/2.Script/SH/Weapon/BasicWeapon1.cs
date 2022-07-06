// #define test
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Photon.Pun;
// using System.Reflection;

// [RequireComponent(typeof(PhotonView))]
// public class BasicWeapon1 : WeaponBase, IInitialize
// {
//     public void Reset()
//     {
// #if UNITY_EDITOR
//         weaponSetting.weaponName = WeaponName.Basic;
//         weaponSetting.maxAmmo = 10;
//         weaponSetting.attackDistance = 10;
//         weaponSetting.attackRate = 0.2f;
//         weaponSetting.damage = 1;
//         handSide = HandSide.Right;

//         if (bullet == null)
//             bullet = Resources.Load("Projectile 10") as GameObject;

//         if (bulletSpawnPoint == null)
//         {
//             Utility.GetBoneTransform(transform.root, HumanBodyBones.RightLowerArm, out Transform forearm);
//             bulletSpawnPoint = Utility.FindChildContainsName(forearm, new string[]{"Basic", "basic"});
//             if (bulletSpawnPoint == null)
//             {
//                 bulletSpawnPoint = new GameObject("BasicWeapon").transform;
//                 bulletSpawnPoint.parent = forearm;
//                 bulletSpawnPoint.localPosition = new Vector3(0.00133327337f,0.0120280581f,0.00210149423f);
//                 bulletSpawnPoint.localEulerAngles = new Vector3(272.714539f,170.087479f,64.0593033f);
//             }
//         }
// #endif
//     }
//     public event Cur_MaxEvent OnValueChange;

//     [Header("SpawnPoint")]
//     public Transform bulletSpawnPoint;

//     [Header("Fire Effects")]
//     [SerializeField] GameObject muzzleFlashEffect;
//     [SerializeField] private GameObject bullet;

//     [Header("Audio Clips")]
//     [SerializeField] private AudioClip onFireSFX;
//     [SerializeField] private AudioClip onReloadSFX;
//     private WaitForEndOfFrame eof = new WaitForEndOfFrame();
//     IEnumerator coroutineHolder;

//     private float CurrentAmmo
//     {
//         get { return weaponSetting.currentAmmo;}
//         set
//         {
//             float prevAmmo = weaponSetting.currentAmmo;
//             weaponSetting.currentAmmo = Mathf.Clamp(value, 0, weaponSetting.maxAmmo);

//             if (prevAmmo != weaponSetting.currentAmmo)
//             {
//                 OnValueChange?.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
//             }
//         }
//     }
    
//     public override void Initialize()
//     {
//         base.Initialize();
//         CurrentAmmo = weaponSetting.maxAmmo;
//     }

//     public override void StartWeaponAction()
//     {
//         if (isReloading)
//             return;

//         coroutineHolder = ContinuousFire();
//         StartCoroutine(coroutineHolder);

//     }

//     public override void StopWeaponAction()
//     {
//         if (coroutineHolder != null)
//             StopCoroutine(coroutineHolder);
//     }

//     public override void StartReload()
//     {
//         if (isReloading)
//             return;

//         StartCoroutine(OnReload());
//     }

//     IEnumerator ContinuousFire()
//     {
//         while(true)
//         {
//             yield return eof;
//             OnAttack();
//         }
//     }
//     [SerializeField] LayerMask hitLayers;

//     private void OnAttack()
//     {
//         // if not ready-to-shoot, return
//         if (Time.time - lastAttackTime < weaponSetting.attackRate || CurrentAmmo <= 0)
//             return;
        
//         lastAttackTime = Time.time;
//         CurrentAmmo--;

//         if (Physics.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.forward, out RaycastHit rayHit, weaponSetting.attackDistance, hitLayers, QueryTriggerInteraction.Collide))
//         {
//             print("RayHit");
//             PhotonView pv = rayHit.collider.GetComponent<PhotonView>();
//             if (pv?.ViewID > 0 == false)
//                 rayHit.collider.GetComponent<IDamageable>()?.TakeDamage(weaponSetting.damage);
           
//             photonView.CustomRPC(this, "RPCAttack", RpcTarget.All, pv?.ViewID, bulletSpawnPoint.position, rayHit.point-bulletSpawnPoint.forward*0.1f);
//         }

    

//         if (CurrentAmmo <= 0)
//             StartReload();
//     }
// public LineRenderer lr;
//     [PunRPC]
//     private void RPCAttack(int viewID, Vector3 a, Vector3 hitPoint)
//     {
//         PhotonView pv = PhotonNetwork.GetPhotonView(viewID);
//         pv?.GetComponent<IDamageable>()?.TakeDamage(weaponSetting.damage);
        
//         ObjectPooler.instance.SpawnFromPool<RailGun>(bullet.name, Vector3.zero, false).OnShoot(a, hitPoint);

//         StartCoroutine(OnMuzzleFlashEffect());
//         PlaySound(onFireSFX);
//     }

//     IEnumerator OnMuzzleFlashEffect()
//     {
//         if (muzzleFlashEffect == null) yield break;

//         muzzleFlashEffect.SetActive(true);
//         yield return new WaitForSeconds(weaponSetting.attackRate * 0.6f);
//         muzzleFlashEffect.SetActive(false);
//     }

//     IEnumerator OnReload()
//     {
//         isReloading = true;
//         WeaponSystem.instance.LockWeapon(weaponSetting.weaponName);

//         yield return new WaitForSeconds(2f); // Reloading Procedure

//         isReloading = false;
//         WeaponSystem.instance.UnlockWeapon(weaponSetting.weaponName);
//         CurrentAmmo = weaponSetting.maxAmmo;
//     }
// }
