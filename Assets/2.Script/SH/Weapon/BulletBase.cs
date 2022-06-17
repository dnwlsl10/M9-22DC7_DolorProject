using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public float damage;
    public virtual void GiveDamage(float damage){}

    protected void OnCollisionEnter(Collision other) 
    {
        if (other.transform.root.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }

        ObjectPooler.ReturnToPool(gameObject);
    }
}
