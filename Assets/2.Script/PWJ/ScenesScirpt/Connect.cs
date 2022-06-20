using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using  UnityEngine.SceneManagement;

public class Connect : MonoBehaviourPunCallbacks
{
    public System.Action OnCompelet;
    private UserInfo userInfo;
    private RobotData robotData;
    public GameObject playerPrefab;
    public Transform target;
    private readonly string gameVersion = "v1.0";
    public bool isTest;
    public UIGame uIGame;

    public void Start(){
        
        if (isTest)
        {
            Init();
        }
    }
    
    private void Update() {
        GameStart();
    }

    public void Init(UserInfo userInfoData = null){

        this.userInfo = userInfoData;
        
        if(!isTest){
            DataManager.GetInstance().LoadDatas();
            this.robotData = DataManager.GetInstance().dicRobotDatas[userInfo.userId];
        }

        PhotonNetwork.LogLevel = PunLogLevel.Full;
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 20;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster(){

        Debug.Log("1.연결");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby(){

        Debug.Log("2.로비");
        if (isTest)
        {
           var obj = Instantiate(playerPrefab, target.transform.position, Quaternion.identity);
        }
        else
        {
            var prefab = Resources.Load<GameObject>("Prefab/" + robotData.connect_name);
            var obj = Instantiate<GameObject>(prefab, target.transform.position, Quaternion.identity);
        }

        uIGame.Init(this);
    }

    public void OnQuickStart(){
        if(!PhotonNetwork.InRoom){
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnCreatedRoom(){
        Debug.Log("3.방생성");
    }

    public override void OnJoinedRoom(){
        Debug.Log("4.방 접속");

        if(!PhotonNetwork.IsMasterClient)
            GameStart();
    }

    public void GameStart(){

        if (isTest && PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                SceneManager.LoadScene("InGame");
            }
        }
        if(!isTest && PhotonNetwork.InRoom ) 
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2){
                OnChangeScene();
            }
        }
    }

    string roomName;

    public override void OnJoinRandomFailed(short returnCode, string message){
        
        Debug.Log("방 없음");

        RoomOptions roomOpt = new RoomOptions();
        roomOpt.MaxPlayers = 2;
        roomOpt.IsVisible = true;
        roomOpt.IsOpen = true;

        this.roomName = "Room_" + Random.Range(1, 100);
        PhotonNetwork.JoinOrCreateRoom(this.roomName, roomOpt  ,null);
    }

    private void OnChangeScene(){
        OnCompelet();
    }
}
