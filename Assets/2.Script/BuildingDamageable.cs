using UnityEngine;
using Photon.Pun;

public class BuildingDamageable : MonoBehaviourPun, IDamageable
{
    [SerializeField] float hp = 100;
    [SerializeField] int buildingIndex;
    bool canTakeDmg = true;
    public void TakeDamage(float damage, Vector3 position)
    {
        photonView.CustomRPC(this, "BDmg", RpcTarget.MasterClient, damage, position);
    }

    [PunRPC]
    private void BDmg(float damage, Vector3 point)
    {
        if (canTakeDmg == false) return;
        
        hp -= damage;
        Debug.Log("Current HP : " + hp);
        if (hp <= 0)
        {
            canTakeDmg = false;
            photonView.CustomRPC(this, "Collapse", RpcTarget.AllViaServer, point);
        }
    }

    [PunRPC]
    private void Collapse(Vector3 position)
    {
        BuildingManager.instance.TryCollapse(new Building(transform, position, buildingIndex));
    }
}
