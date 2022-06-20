using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PartsHP : MonoBehaviour, IDamageable
{
    public event Cur_MaxEvent OnHpValueChange;
    public float maxHP;
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
            hp = (value <= 0 ? 0 : (value > maxHP ? maxHP : value));

            if (prevHp != hp)
            {
                if (hp == 0) 
                {
                    hpValueFixed = true;
                    OnDeath();
                }
            }
        }
    }
    private Status status;
    private void Awake() 
    {
        HP = maxHP;
        status = transform.root.GetComponent<Status>();
    }

    private void OnDeath()
    {

    }

    public void TakeDamage(float damage, GameObject hitObject = null)
    {
        print("Damage on " + gameObject.name);
        HP -= damage;
        status.TakeDamage(damage);
    }
}
