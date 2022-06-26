using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechLand : MonoBehaviour, IInitialize
{
    public void Reset()
    {
        groundLayer = LayerMask.GetMask("Ground");

        if (componentsAfterStartScene.Count == 0)
        {
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<HandIK>(true));
            // componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<MechMovementController>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<WeaponBase>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<WeaponSystem>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<CrossHair>(true));
            componentsAfterStartScene.AddRange(transform.root.GetComponentsInChildren<IKWeight>(true));
        }

        groundDetectDistance = 0.6f;
    }

    [ContextMenu("land")]
    void land()
    {
        StartFalling();
    }

    private Animator anim;
    public LayerMask groundLayer;
    public List<Behaviour> componentsAfterStartScene;
    public float groundDetectDistance = 1;
    MechScriptManager scriptManager;
    private void Awake() 
    {
        anim = GetComponent<Animator>();
        scriptManager = GetComponent<MechScriptManager>();

        StartFalling();
    }
    public void StartFalling()
    {
        StartCoroutine(CheckGroundDistance());
    }

    IEnumerator CheckGroundDistance()
    {
        scriptManager.DisableScripts(ref componentsAfterStartScene);
        anim.SetLayerWeight(1, 0);

        anim.CrossFade("Falling", 0.2f, 0);

        // Wait until distance from ground less than threshold;
        while (Physics.Raycast(transform.position, -transform.up, groundDetectDistance, groundLayer) == false)
            yield return null;

        anim.SetTrigger("Land");

        yield return new WaitForSeconds(3f);

        scriptManager.EnableScripts(ref componentsAfterStartScene);
        for (float f = 0; f < 1; f += Time.deltaTime)
        {
            anim.SetLayerWeight(1, f/1);
            yield return null;
        }
        anim.SetLayerWeight(1, 1);

        Destroy(this);
    }
}
