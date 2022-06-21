using UnityEngine;

[RequireComponent(typeof(Photon.Pun.PhotonView))]
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

    public void TakeDamage(float damage)
    {
        PassDamage(damage);
    }

    public void PassDamage(float damage)
    {
        target.TakeDamage(damage);
    }
}
