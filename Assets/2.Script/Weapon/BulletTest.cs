using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : MonoBehaviour
{
    // Update is called once per frame
    public float speed = 1;
    void Awake()
    {
        print("Awake Bullet Transform : " + transform.position + " // " + transform.eulerAngles);
    }

    void Start()
    {
        print("Start Bullet Transform : " + transform.position + " // " + transform.eulerAngles);

    }
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
