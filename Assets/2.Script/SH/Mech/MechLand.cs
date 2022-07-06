using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechLand : MonoBehaviour, IInitialize
{
    public void Reset()
    {
#if UNITY_EDITOR
        groundLayer = LayerMask.GetMask("Ground"); groundDetectDistance = 0.6f; if (componentsAfterStartScene.Count == 0) {
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
    private void Awake() 
    {
        anim = GetComponent<Animator>();
        foreach (var component in componentsAfterStartScene)
            if(component) component.enabled = false;
        
        StartFalling();
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
        while (Physics.Raycast(transform.position, -transform.up, groundDetectDistance, groundLayer) == false)
            yield return null;

        anim.SetTrigger("Land");
        AudioPool.instance.Play(clip.name, 2, transform.position);
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
