using UnityEngine;

// [RequireComponent(typeof(Photon.Pun.PhotonView))]
public class DamagePasser : MonoBehaviour, IDamageable
{
    public IDamageable target;

    private void Awake() 
    {
        FindTarget();
    }

    private void FindTarget()
    {
        target = transform.root.GetComponent<IDamageable>();
    }

    public bool TakeDamage(float damage)
    {
        PassDamage(damage);
        return true;
    }

    public void PassDamage(float damage)
    {
        target.TakeDamage(damage);
    }
}
