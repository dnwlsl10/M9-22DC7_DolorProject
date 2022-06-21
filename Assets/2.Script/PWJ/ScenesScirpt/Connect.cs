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

    [Header("Data")]
    [SerializeField]
    private UserInfo userInfo;
    private RobotData robotData;

    [Header("")]
    public GameObject playerPrefab;
    public Transform target;

    [Header("")]
    public bool isTest;
    public UIGame uIGame;

    private string roomName;
    private readonly string gameVersion = "v1.0";



    public void Start(){
        
        if (isTest)
        {
            Init();
        }

        UIGame.OnPracticeMode += OnCreateParcticeRoom;
        UIGame.OnLobby += OnLeftPracticeRoom;
        UIGame.OnQucikMatch += OnCreateQuickMatchRomm;
    }

    public void Init(UserInfo userInfoData = null)
    {
        this.userInfo = userInfoData;

        if (!isTest)
        {
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

    public override void OnDisable()
    {
        UIGame.OnPracticeMode -= OnCreateParcticeRoom;
        UIGame.OnLobby -= OnLeftPracticeRoom;
        UIGame.OnQucikMatch -= OnCreateQuickMatchRomm;
    }

    public void OnCreateQuickMatchRomm(){

    }
    public void OnCreateParcticeRoom(){
      
        if (uIGame.isPractice)
        { 
            RoomOptions roomOpt = new RoomOptions();
            roomOpt.MaxPlayers = 2;
            roomOpt.IsVisible = false;
            roomOpt.IsOpen = false;
            this.roomName = "PracticeRoom_" + Random.Range(1, 100);
            Debug.Log(roomName);
            PhotonNetwork.CreateRoom(this.roomName, null);
        }
    }

    public void OnLeftPracticeRoom() => OnLeftRoom();
    public void OnLeftQuickMatchRoom() => OnLeftRoom();

    public override void OnLeftRoom(){
        
        if(!uIGame.isPractice) Debug.Log("PracticeRoom => Lobby");
    }

    private void Update() => GameStart();

    public override void OnConnectedToMaster(){

        Debug.Log("Conneect");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby(){

        Debug.Log("Lobby");
        if(isTest) Instantiate(playerPrefab, target.transform.position, Quaternion.identity);

        else
        {
            var prefab = Resources.Load<GameObject>("Prefab/" + robotData.connect_name);
            var obj = Instantiate<GameObject>(prefab, target.transform.position, Quaternion.identity);
         
        }
        uIGame.Init(this);
    }

    public void OnQuickStart(){
        if(!PhotonNetwork.InRoom) PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreatedRoom() => Debug.Log("방생성");
      
    public override void OnJoinedRoom(){
        
        Debug.Log("4.방 접속");
        if(!uIGame.isPractice) Debug.Log("Lobby => PraticeRoom");
        if(!PhotonNetwork.IsMasterClient) GameStart();
    }

    public void GameStart(){

        if (isTest && PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2) SceneManager.LoadScene("InGame");
        }
        if(!isTest && PhotonNetwork.InRoom ) 
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2) OnChangeScene();
        }
    }



    public override void OnJoinRandomFailed(short returnCode, string message){
        
        Debug.Log("방 없음");

        RoomOptions roomOpt = new RoomOptions();
        roomOpt.MaxPlayers = 2;
        roomOpt.IsVisible = true;
        roomOpt.IsOpen = true;

        this.roomName = "Room_" + Random.Range(1, 100);
        PhotonNetwork.JoinOrCreateRoom(this.roomName, roomOpt  ,null);
    }

    private void OnChangeScene() => OnCompelet();
}
