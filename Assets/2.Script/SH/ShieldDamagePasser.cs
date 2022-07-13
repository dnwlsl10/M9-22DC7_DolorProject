using UnityEngine;

public class ShieldDamagePasser : MonoBehaviour, IDamageable
{
    [SerializeField] private SkillShield target;

    public void TakeDamage(float damage, Vector3 position)
    {
        if (target != null) target.TakeDamage(damage, position);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            WeaponSystem.instance.GetComponentInChildren<SkillShield>();
        }
    }

}
