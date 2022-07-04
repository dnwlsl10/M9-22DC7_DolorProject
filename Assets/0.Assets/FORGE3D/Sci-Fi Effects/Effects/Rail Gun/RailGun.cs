using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun : MonoBehaviour
{
    LineRenderer lr;
    [SerializeField] float disableTime;
    // WaitForSeconds wfs;
    private void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    public void OnShoot(Vector3 a, Vector3 hitPoint)
    {
        lr.SetPosition(0, a);
        lr.SetPosition(1, hitPoint);
        lr.enabled = true;
        gameObject.SetActive(true);

        StartCoroutine(DisableAfter());
    }

    IEnumerator DisableAfter()
    {
        yield return new WaitForSeconds(disableTime);
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        lr.enabled = false;
    }
}
