using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkTest : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject simulator;
    public bool testMode;

    private void Start() 
    {
        // PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
        Connect();
    }

#region NetworkConnect
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        print(PhotonNetwork.CountOfPlayers);
        JoinRandomRoom();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {

    }
#endregion

#region Room
    public void CreateRoom() 
    {
        PhotonNetwork.CreateRoom("Room" + Random.Range(0, 100), new RoomOptions {MaxPlayers = 2});
    }
    
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public GameObject objPool;
    public override void OnJoinedRoom()
    {
        print("Joined Room" + PhotonNetwork.CurrentRoom.PlayerCount);
        // PhotonNetwork.Instantiate(testMode ? "Prefab/Mech_Test" : "Prefab/Mech_ForUse", Vector3.zero, Quaternion.identity);
        // PhotonNetwork.Instantiate("Prefab/Mech_Test", Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        // Instantiate(objPool);
        if (testMode) Invoke("SpawnSimulator", 1);
    }
    private void SpawnSimulator()
    {
        Instantiate(simulator);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Room not Exist");
        CreateRoom();
    }

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
        
    //}


    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
        
    //}

    //public override void OnMasterClientSwitched(Player newMasterClient)
    //{

    //}

#endregion
}