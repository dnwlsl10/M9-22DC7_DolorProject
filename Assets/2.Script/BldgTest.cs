using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BldgTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        other.gameObject.GetComponent<BuildingDamageable>()?.TakeDamage(1000, other.GetContact(0).point);
    }
}
