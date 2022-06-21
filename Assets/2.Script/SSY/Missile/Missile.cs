using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private Vector3[] path;

    [SerializeField]
    private GameObject target;

    bool missilePointOn;

    public void SetPath(Vector3[] path)
    {
        this.path = path;
    }

    void Start()
    {
        if (missilePointOn)
        {
            Vector3 p1 = path[0];
            Vector3 p2 = path[1];
            Vector3 dir = p2 - p1;
            transform.forward = dir;
        }
    }

    void Update()
    {
        MissilePoint();
    }

    float t;
    public float speed = 5;
    int index = 0;
    public float timeSpeed = 5;

    void MissilePoint()
    {
        missilePointOn = true;
        if (index >= path.Length - 1)
        {
            return;
        }
        Vector3 p1 = path[index];
        Vector3 p2 = path[index + 1];
        transform.position = Vector3.Lerp(p1, p2, t);
        Vector3 dir = p2 - p1; //진행방향
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.forward);
        transform.rotation = Quaternion.Lerp
        (transform.rotation, targetRotation, Time.deltaTime * 5);
        if (index < path.Length - 1)
        {
            t += Time.deltaTime * timeSpeed;
            timeSpeed+=Time.deltaTime;
            if (t > 1)
            {
                index++;
                t = 0;

            }
        }
    }
    //부딪혔을 때
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
