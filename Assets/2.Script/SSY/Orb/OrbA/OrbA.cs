using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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

        visualEffect.SetFloat("Size", 2f);
        yield return new WaitForSeconds(6f);
        visualEffect.SetFloat("Size", 0f);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    public override void OrbFire()
    {
        base.OrbFire();
        StartCoroutine(OrbSize());
    }
}
