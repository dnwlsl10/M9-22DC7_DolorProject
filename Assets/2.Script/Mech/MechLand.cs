using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechLand : MonoBehaviour
{

    [ContextMenu("land")]
    void land()
    {
        StartFalling();
    }

    private Animator anim;
    public LayerMask groundLayer;
    public Behaviour[] componentsAfterStartScene;
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

        anim.SetLayerWeight(1, 1);
        scriptManager.EnableScripts(ref componentsAfterStartScene);
    }
}
