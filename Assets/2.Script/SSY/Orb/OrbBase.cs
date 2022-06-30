using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class OrbBase : MonoBehaviour
{
    [SerializeField]
    protected float onShootSpeed;
    protected float orbSpeed;
   

    protected void Update()
    {
        OrbMoving();
    }
    protected void OnEnable() {
        Init();
    }

    protected virtual void Init()
    {
        orbSpeed = 0;
    }
    protected void OrbMoving()
    {
        Vector3 dir = this.transform.forward;
        transform.position += dir * orbSpeed * Time.deltaTime;
    }
    
    public virtual void OrbFire()
    {
        this.transform.SetParent(null);
        orbSpeed = onShootSpeed;
    }
}
