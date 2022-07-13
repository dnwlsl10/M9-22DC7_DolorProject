using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Photon.Pun;

public class OrbA : OrbBase
{
    public VisualEffect visualEffect;
    protected override void Init()
    {
        base.Init();
        onShootSpeed = 2f;
        visualEffect.SetFloat("Size", 1f);
    }
    IEnumerator OrbSize()
    {
        yield return new WaitForSeconds(3f);

        visualEffect.SetFloat("Size", 3f); //변화된 오브의 크기
        yield return new WaitForSeconds(10f);
        visualEffect.SetFloat("Size", 0f);
        yield return new WaitForSeconds(1f);
        
        gameObject.SetActive(false);
    }

    protected override void RPCFire(Vector3 shootPosition, Vector3 forward)
    {
        base.RPCFire(shootPosition, forward);
        StartCoroutine(OrbSize());
    }
}
