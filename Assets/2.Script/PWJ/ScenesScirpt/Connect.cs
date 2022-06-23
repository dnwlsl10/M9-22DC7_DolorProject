using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using  UnityEngine.SceneManagement;

public enum eRoomMode { Lobby, PracticeRoom, QuickMatchRoom }
public class Connect : MonoBehaviourPunCallbacks
{
    [Header("Data")]
    [SerializeField]
    private UserInfo userInfo;
    private RobotData robotData;
    private GameObject robot;

    [Header("Assigned")]
    public GameObject playerPrefab;
    public Transform target;
    public InGame inGame;
    [Header("Test")]
    public bool isTest;

    public System.Action OnCompelet;
    private string roomName;
    private readonly string gameVersion = "v1.0";
    private eRoomMode roomMode;

    private void EventHandler()
    {
        InGame.OnPracticeMode += CustomCreatedRoom;
        InGame.OnQucikMatch += CustomCreatedRoom;
        InGame.OnLobby += OnJoinedLobby;
    }
    public void Start() {if(isTest) Init();}
   
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
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby(TypedLobby.Default);
    public override void OnJoinedLobby()
    {
        this.roomMode = eRoomMode.Lobby;
        if (isTest)
        {
            robot = Instantiate(playerPrefab, target.transform.position, Quaternion.identity);
        }
        else{
            var prefab = Resources.Load<GameObject>("Prefab/" + robotData.connect_name);
            robot = Instantiate<GameObject>(prefab, target.transform.position, Quaternion.identity);
        }

        inGame.Init(robot);
        EventHandler();
    }
    public void CustomCreatedRoom(eRoomMode room)
    {
        switch(room){
            case eRoomMode.Lobby: return;
            case eRoomMode.PracticeRoom: {
                    this.roomMode = room;
                    RoomOptions roomOpt = new RoomOptions();
                    roomOpt.MaxPlayers = 2;
                    roomOpt.IsVisible = false;
                    roomOpt.IsOpen = false;
                    this.roomName = roomMode.ToString();
                    PhotonNetwork.CreateRoom(this.roomName, null);
            }
            break;
            case eRoomMode.QuickMatchRoom:{
                    this.roomMode = room;
                    RoomOptions roomOpt = new RoomOptions();
                    roomOpt.MaxPlayers = 2;
                    roomOpt.IsVisible = true;
                    roomOpt.IsOpen = true;
                    this.roomName = roomMode.ToString();
                    PhotonNetwork.CreateRoom(this.roomName, null);
            }
            break;
        }
    }
    public void OnLeftCustomRoom() => OnLeftRoom();

    public override void OnLeftRoom(){
        NetworkObjectPool.instance.DestroyPool();
        Debug.Log(roomMode);
    }
    private void Update() => GameStart();

    public void StartQuickMatch(){
        if(this.roomMode != eRoomMode.QuickMatchRoom) PhotonNetwork.JoinRandomRoom();
    }

    public override void OnCreatedRoom() => Debug.Log(roomMode);
      
    public override void OnJoinedRoom(){
         Debug.Log(roomMode);
        if(!PhotonNetwork.IsMasterClient && roomMode == eRoomMode.QuickMatchRoom) GameStart();
    }

    public void GameStart(){

        if(isTest&& PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2) 
            {
                inGame.FindOtherPlayer(() =>{
                    if(isTest) SceneManager.LoadScene("InGame");
                    else OnChangeScene();
                });
            }
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) => CustomCreatedRoom(eRoomMode.QuickMatchRoom);
    private void OnChangeScene() => OnCompelet();

    public override void OnDisable() {
        InGame.OnPracticeMode -= CustomCreatedRoom;
        InGame.OnQucikMatch -= CustomCreatedRoom;
        InGame.OnLobby -= OnJoinedLobby;
        NetworkObjectPool.instance.DestroyPool();
    }
}
