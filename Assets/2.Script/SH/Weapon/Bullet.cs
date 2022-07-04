using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        rb.velocity = transform.forward * speed;
    }

    private void OnDisable() {
        print("Disable");
    }
}
