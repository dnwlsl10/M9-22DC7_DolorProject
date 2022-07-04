using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrb : UIBase
{
    [Header("Pivot")]
    public Transform originOrbA;
    public Transform originOrbB;
    public Transform originOrbC;
    [Header("OrbB")]
    public GameObject orbBAvailable;
    public GameObject orbBReload;
    [Header("OrbC")]
    public GameObject orbCAvailable;
    public GameObject orbCReload;


    void Start()
    {
        listTest.Add(originOrbA);
        listTest.Add(originOrbB);
        listTest.Add(originOrbC);


        orbBAvailable.SetActive(false);
        orbBReload.SetActive(true);

        orbCAvailable.SetActive(false);
        orbCReload.SetActive(true);
    }
    List<Transform> listTest;
    public void Update(){
        
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            //터치
            StartCoroutine(OnLerpTest());
           
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //터치 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //터치 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //터치 
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //터치 
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //터치 
        }
    }
    Vector3 tmp;
    Vector3 tmpB;
    Vector3 tmpC;

    IEnumerator OnLerpTest(){

        tmp = originOrbA.position;
        tmpB = originOrbB.position;
        tmpC = originOrbC.position;
        while(Vector3.Distance(originOrbA.position,originOrbC.position) > 0)
        {
            yield return null;
        
            originOrbA.position = Vector3.Lerp(originOrbA.position, tmpC , Time.deltaTime);
            originOrbC.position = Vector3.Lerp(originOrbC.position, tmpB, Time.deltaTime);
            originOrbB.position = Vector3.Lerp(originOrbB.position, tmp , Time.deltaTime);
        }
        tmp = originOrbA.position;
        tmpB = originOrbB.position;
        tmpC = originOrbC.position;
    }
    public override void EventValue(float current, float max)
    {

    }
}
