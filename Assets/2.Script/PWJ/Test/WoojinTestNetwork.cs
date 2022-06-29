using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class WoojinTestNetwork : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject simulator;
    public bool testMode;
    void Awake()
    {
        //화면 비율 960 540 , 전체화면 x 
        Screen.SetResolution(960, 540, false);
        //동기화 설정 
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AutomaticallySyncScene = true;
        if (testMode) Invoke("SpawnSimulator", 1);

        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        Debug.Log("접속");
    }


     private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)){
            Connect();
        }   
    }
    public void Connect() => PhotonNetwork.ConnectUsingSettings();  

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 6 }, null);  
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방입장");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       Debug.Log("접속");
       if(PhotonNetwork.IsMasterClient) PhotonNetwork.LoadLevel("TestScene_Woojin 1");

    }

    private void Test(){
      
    }
    private void SpawnSimulator()
    {
        Instantiate(simulator);
    }
}
