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
    [SerializeField] byte maxPlayer;
    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Confined;
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1.0";
    }

#region NetworkConnect
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        maxPlayer = GameObject.FindObjectOfType<Dropdown>().value == 0 ? (byte)1 : (byte)2;
        // print(PhotonNetwork.CountOfPlayers);
        // print(maxPlayer);
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
        PhotonNetwork.CreateRoom("Room" + 99, new RoomOptions {MaxPlayers = maxPlayer});
    }
    
    public void JoinRandomRoom() => PhotonNetwork.JoinOrCreateRoom("Room99", new RoomOptions {MaxPlayers = maxPlayer}, TypedLobby.Default);

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom()
    {
        // print("Joined Room" + PhotonNetwork.CurrentRoom.PlayerCount);
        if (maxPlayer == 1) OnPlayerEnteredRoom(null);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // print("Room not Exist");
        CreateRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayer && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("GameStart", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    void GameStart()
    {
        StartCoroutine(IEGameStart());
    }

    IEnumerator IEGameStart()
    {
        for (float f = 0; f < 3; f += Time.deltaTime)
        {
            // print(Mathf.RoundToInt(f));
            yield return null;
        }

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(1);
    }

    


    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
        
    //}

    //public override void OnMasterClientSwitched(Player newMasterClient)
    //{

    //}

#endregion
}