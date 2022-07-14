using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
    private void BDmg(float damage, Vector3 point, PhotonMessageInfo info)
    {
        if (canTakeDmg == false) return;

        hp -= damage;
        // Debug.Log("Current HP : " + hp);
        if (hp <= 0)
        {
            canTakeDmg = false;
            photonView.CustomRPC(this, "Collapse", RpcTarget.AllViaServer, point);
            photonView.CustomRPC(this, "GMUp", info.Sender);
        }
    }

    [PunRPC]
    private void Collapse(Vector3 position)
    {
        BuildingManager.instance.TryCollapse(new Building(transform, position, buildingIndex));
    }

    [PunRPC]
    private void GMUp()
    {
        WeaponSystem.instance.GetComponent<GuidedMissile>().GetGauge(5f);
    }
}
