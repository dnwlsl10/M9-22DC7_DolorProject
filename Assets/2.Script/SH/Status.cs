using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Status : MonoBehaviourPun, IDamageable
{
    public event Cur_MaxEvent OnValueChange;
    public int maxHP = 100;
    [SerializeField]
    private float hp;
    private bool hpValueFixed;
    public float HP
    {
        get{return hp;}
        private set
        {
            if (hpValueFixed) return;

            float prevHp = hp;
            hp = Mathf.Clamp(value, 0, maxHP);

            if (prevHp != hp)
            {
                OnValueChange?.Invoke(hp, maxHP);
                if (hp == 0) OnDeath();
            }
        }
    }

    private void Start() {
        HP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        photonView.CustomRPC(this, "TD_RPC", photonView.Owner, damage);
    }

    [PunRPC]
    private void TD_RPC(float damage)
    {
        Debug.Log("Damage" + damage);
        HP -= damage;
    }

    private void OnDeath()
    {
        hpValueFixed = true;
    }
}
