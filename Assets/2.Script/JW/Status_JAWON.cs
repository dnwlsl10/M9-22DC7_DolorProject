using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_JAWON : MonoBehaviour, IDamageable
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TakeDamage(4);
        }
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     UIManager.Instance.UpdateUltimateSkillGauge();
        // }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage" + damage);
        HP -= damage;
        // UIManager.Instance.UpdateHPGauge(damage);
    }

    private void OnDeath()
    {

    }
}
