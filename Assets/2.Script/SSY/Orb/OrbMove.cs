using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OrbMove : MonoBehaviour
{
    public float shootSpeed;
    float orbSpeed;
    public VisualEffect visualEffect;

    void Update()
    {
        OrbMoving();
    }
    void OrbMoving()
    {
        Vector3 dir = this.transform.forward;
        transform.position += dir * orbSpeed * Time.deltaTime;
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
    public void OrbSpeedChange()
    {
        orbSpeed = shootSpeed;
        StartCoroutine("OrbSize");
    }
}
