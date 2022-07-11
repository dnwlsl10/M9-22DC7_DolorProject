using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Status : MonoBehaviourPun, IDamageable
{
    public event System.Action<float, float> OnValueChange;
    [SerializeField] private int maxHP = 100;
    [SerializeField] private float hp; // show inspector
    public bool lockHp;
    public float HP
    {
        get{return hp;}
        private set
        {
            if (lockHp) return;

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

    public void TakeDamage(float damage, Vector3 position)
    {
        if (lockHp == false)
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
        lockHp = true;
        GameManager.instance?.OnPlayerDeath();
    }
}
