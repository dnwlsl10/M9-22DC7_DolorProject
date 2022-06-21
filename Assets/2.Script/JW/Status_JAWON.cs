using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_JAWON : MonoBehaviour, IDamageable
{
    public delegate void HPEvent(float curHP, float MaxHP);
    public static event HPEvent OnHPChange;
    public int maxHP = 100;
    private float hp = 100;
    // public float HP
    // {
    //     get{return hp;}
    //     private set
    //     {
    //         if (value > maxHP)
    //             hp = maxHP;
    //         else if (value <= 0)
    //         {
    //             hp = 0;
    //             OnDeath();
    //         }
    //         else
    //             hp = value;
    //     }
    // }

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage" + damage);
        hp -= damage;
        OnHPChange(hp,maxHP);
    }

    private void OnDeath()
    {

    }

    private void Update() {
        if(Input.GetMouseButtonDown(0))
        {
            TakeDamage(5);
        }
    }
}
