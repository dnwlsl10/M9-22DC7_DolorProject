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
    bool hpValueFixed;
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
        hp = maxHP;
        OnValueChange?.Invoke(hp, maxHP);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Damage" + damage);
        HP -= damage;
        //StartCoroutine(test());
    }

    private void OnDeath()
    {
        hpValueFixed = true;
    }

    // IEnumerator test()
    // {
    //     Renderer[] renderers = GetComponentsInChildren<Renderer>();

    //     Color origin = renderers[0].material.color;

    //     foreach(var renderer in renderers)
    //     {
    //         renderer.material.color = Color.red;
    //     }

    //     yield return new WaitForSeconds(0.01f);

    //     foreach(var renderer in renderers)
    //     {
    //         renderer.material.color = origin;
    //     }
    // }
}
