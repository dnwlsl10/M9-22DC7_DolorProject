using UnityEngine;
using Photon.Pun;

public class BuildingDamageable : MonoBehaviourPun, IDamageable
{
    [SerializeField] float hp = 100;
     
    public void TakeDamage(float damage, Vector3 position)
    {
        photonView.CustomRPC(this, "BDmg", RpcTarget.MasterClient, damage, position);
    }

    [PunRPC]
    private void BDmg(float damage, Vector3 point)
    {
        hp -= damage;
        Debug.Log("Current HP : " + hp);
        if (hp <= 0)
        {
            photonView.CustomRPC(this, "Collapse", RpcTarget.AllViaServer, point);
        }
    }

    [PunRPC]
    private void Collapse(Vector3 position)
    {
        BuildingManager.instance.TryCollapse(new Building(gameObject, position));
    }
}
