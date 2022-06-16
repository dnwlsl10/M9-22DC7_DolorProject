using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    // Update is called once per frame
    public float speed = 1;
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
