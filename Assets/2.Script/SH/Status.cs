using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour, IDamageable
{
    public event Cur_MaxEvent OnHpValueChange;
    public int maxHP = 100;
    private float hp;
    bool hpValueFixed;
    public float HP
    {
        get{return hp;}
        private set
        {
            if (hpValueFixed) return;

            float prevHp = hp;
            hp = (value <= 0 ? 0 : (value > maxHP ? maxHP : value));

            if (prevHp != hp)
            {
                OnHpValueChange?.Invoke(hp, maxHP);
                if (hp == 0) OnDeath();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage" + damage);
        HP -= damage;
    }

    private void OnDeath()
    {
        hpValueFixed = true;
    }
}
