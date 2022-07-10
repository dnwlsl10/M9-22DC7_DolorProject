using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Building
{
    public GameObject building; public Vector3 hitPosition;
    public Building(GameObject building, Vector3 hitPosition)
    { this.building = building; this.hitPosition = hitPosition; }
}
public class BuildingManager : MonoBehaviour
{

    public static BuildingManager instance;
    Queue<Building> collapseQ = new Queue<Building>();
    int currentCollapsingBuilding;

    private void Awake() {
        instance = this;
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
        while(currentCollapsingBuilding < 2)
            yield return null;
        
        currentCollapsingBuilding++;
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
    private void Collapse(Building bdg)
    {
        currentCollapsingBuilding++;

        int colNum = Physics.OverlapSphereNonAlloc(bdg.hitPosition, scanRadius, colliders, layer, QueryTriggerInteraction.Ignore);
        bdg.building.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < colNum; i++)
        {
            colliders[i].attachedRigidbody.isKinematic = false;
            colliders[i].attachedRigidbody.AddExplosionForce(100, bdg.hitPosition, scanRadius, 0, ForceMode.Impulse);
        }
    }
}
