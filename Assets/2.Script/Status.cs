using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour, IDamageable
{
    public int maxHP = 100;
    private float hp;
    public float HP
    {
        get{return hp;}
        private set
        {
            if (value > maxHP)
                hp = maxHP;
            else if (value <= 0)
            {
                hp = 0;
                OnDeath();
            }
            else
                hp = value;
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage" + damage);
        HP -= damage;
    }

    private void OnDeath()
    {

    }
}
