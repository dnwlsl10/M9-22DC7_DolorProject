using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbFire : MonoBehaviour
{
    public GameObject firePosition;
    public GameObject orbFactory;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OrbShoot();
    }
    void OrbShoot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject orb = Instantiate(orbFactory);
            orb.transform.position = firePosition.transform.position;
        }

    }
}
