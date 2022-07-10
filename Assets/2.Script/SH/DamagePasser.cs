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

    public void TakeDamage(float damage, Vector3 position)
    {
        PassDamage(damage, position);
    }

    public void PassDamage(float damage, Vector3 position)
    {
        target.TakeDamage(damage, position);
    }
}
