using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Building
{
    public Transform building; public Vector3 hitPosition; public int buildingType;
    public Building(Transform building, Vector3 hitPosition, int buildingType)
    { this.building = building; this.hitPosition = hitPosition; this.buildingType = buildingType; }
}
public class BuildingManager : MonoBehaviour
{

    public static BuildingManager instance;
    Queue<Building> collapseQ = new Queue<Building>();
    int currentCollapsingBuilding;
    [SerializeField] int explosionForce;

    private void Awake() {
        instance = this;
        colliders = new Collider[maxDetectCollider];
    }

    public void TryCollapse(Building bdg)
    {
        if (currentCollapsingBuilding < 2)
            Collapse(bdg);
        else
        {
            collapseQ.Enqueue(bdg);
            if (coroutine == null)
            {
                coroutine = WaitCollapse();
                StartCoroutine(coroutine);
            }
        }
    }

    IEnumerator coroutine;
    IEnumerator WaitCollapse()
    {
        while(currentCollapsingBuilding >= 2)
            yield return null;
        
        // currentCollapsingBuilding++;
        Collapse(collapseQ.Dequeue());

        if (collapseQ.Count > 0)
        {
            coroutine = WaitCollapse();
            StartCoroutine(WaitCollapse());
        }
    }

    Collider[] colliders;
    [SerializeField] LayerMask layer;
    [SerializeField] int maxDetectCollider;
    [SerializeField] float scanRadius;
    [SerializeField] GameObject[] fracturedBuilding;
    [SerializeField] AudioClip bdSfx;
    private void Collapse(Building bldg)
    {
        currentCollapsingBuilding++;

        bldg.building.gameObject.SetActive(false);

        var fracturedBldg = ObjectPooler.instance.SpawnFromPool(fracturedBuilding[bldg.buildingType].name, bldg.building.position, bldg.building.rotation);
        fracturedBldg.transform.localScale = bldg.building.lossyScale;
        
        int colNum = Physics.OverlapSphereNonAlloc(bldg.hitPosition, scanRadius, colliders, layer, QueryTriggerInteraction.Ignore);
        if (bdSfx) AudioPool.instance.Play(bdSfx.name, 2 , bldg.building.position);
        print("COLNUM" + colNum);
        for (int i = 0; i < colNum; i++)
        {
            colliders[i].gameObject.layer = 0;
            colliders[i].attachedRigidbody.AddExplosionForce(explosionForce, bldg.hitPosition, scanRadius, 0, ForceMode.Impulse);
        }
        StartCoroutine(DisableAfter(fracturedBldg));
    }

    [SerializeField] float disableTime = 5;
    IEnumerator DisableAfter(GameObject fracturedBuilding)
    {
        yield return new WaitForSeconds(disableTime);
        fracturedBuilding.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        currentCollapsingBuilding--;
    }
}
