using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechLand : MonoBehaviour, IInitialize
{
    [ContextMenu("ADD")]
    public void Reset()
    {
#if UNITY_EDITOR
        // groundLayer = LayerMask.GetMask("Ground"); groundDetectDistance = 0.6f; 
        if (componentsAfterStartScene.Count == 0) {
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<HandIK>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<MechMovementController>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<WeaponBase>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<WeaponSystem>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<CrossHair>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<IKWeight>(true));}
#endif
    }

    private Animator anim;
    [SerializeField] private AudioClip clip;
    public LayerMask groundLayer;
    public List<Behaviour> componentsAfterStartScene;
    public float groundDetectDistance = 1;
    CharacterController cc;
    public GameObject groundVfx;
    private void Awake() 
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        foreach (var component in componentsAfterStartScene)
            if(component) component.enabled = false;
        
        StartFalling();
    }
    [SerializeField] float gravity = 2;
    float fallSpeed = 0;
    private void Update() {
        float deltaTime = Time.deltaTime;
        cc.Move(new Vector3(0, fallSpeed * deltaTime, 0));
        fallSpeed -= deltaTime * 2;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        fallSpeed = 0;
    }

    public void StartFalling() => StartCoroutine(CheckGroundDistance());

    IEnumerator CheckGroundDistance()
    {
        var tmp = GetComponent<RootMotion.Demos.VRIK_PUN_Player>();
        float storage = tmp.proxyMaxErrorSqrMag;
        tmp.proxyMaxErrorSqrMag = 20;

        anim.SetLayerWeight(1, 0);
        anim.CrossFade("Falling", 0.2f, 0);

        // Wait until distance from ground less than threshold;
        while (Physics.Raycast(transform.position, -transform.up, groundDetectDistance * 3, groundLayer) == false)
            yield return null;

if (groundVfx)
    {
            groundVfx.SetActive(true); //이펙트
            Destroy(this.groundVfx , 3f);

    }
        if (clip) AudioPool.instance.Play(clip.name, 2, transform.position);

        while (Physics.Raycast(transform.position, -transform.up, groundDetectDistance, groundLayer) == false)
            yield return null;

        anim.SetTrigger("Land");
        
        tmp.proxyMaxErrorSqrMag = storage;

        yield return new WaitForSeconds(3f);

        foreach (var component in componentsAfterStartScene)
            if(component) component.enabled = true;

        for (float f = 0; f < 1; f += Time.deltaTime)
        {
            anim.SetLayerWeight(1, f);
            yield return null;
        }
        anim.SetLayerWeight(1, 1);

        componentsAfterStartScene = null;
        this.enabled = false;
    }
}
