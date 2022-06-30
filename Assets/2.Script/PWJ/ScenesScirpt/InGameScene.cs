using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : MonoBehaviour
{

    [Header("InGameScene")]
    public System.Action OnComplete; //Scence Change
    public DoorSystem[] doors;
    public Transform playerSpawn;
    public Transform remoteSpawn;
    public GameObject[] maps;
    public bool testMode;
    void Start()
    {
        if(testMode) Init();
    }
    public void Init()
    {
        Debug.Log("Ready");
        
        //

        Debug.Log("Start");
        OpenDoor();
    }
    private void OpenDoor(){
        foreach(var door in doors){
            door.Open(()=>{SetAtiveMap(); });
        }
    }
    private void SetAtiveMap()
    {
        foreach(var map in maps){
            map.SetActive(false);
        }
    }
  
}
