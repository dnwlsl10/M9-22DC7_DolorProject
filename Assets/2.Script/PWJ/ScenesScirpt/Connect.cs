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
    public System.Action<bool> IsMasterClient;
    private WaitForSeconds eof = new WaitForSeconds(2f);
    public Text count;

    private string roomName;
    private readonly string gameVersion = "v1.0";
    private eRoomMode roomMode;

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
        inGame.Init(robot, photonView);
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
                    PhotonNetwork.JoinRandomRoom();
            }
            break;
        }
    }
    public void OnLeftCustomRoom() => OnLeftRoom();

    public override void OnLeftRoom(){
        NetworkObjectPool.instance.DestroyPool();
        Debug.Log(roomMode);
    }

    public override void OnCreatedRoom() => Debug.Log(roomMode);
      
    public override void OnJoinedRoom(){
        IsMasterClient(PhotonNetwork.IsMasterClient);
    }

    private void Update() {
        Debug.Log(PhotonNetwork.LevelLoadingProgress);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient) inGame.DetectRemotePlayerJoin();
        photonView.RPC("GameStartRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void GameStartRPC() => StartCoroutine(OnStartCount());
    IEnumerator OnStartCount()
    {
        count.text = "3";
        yield return eof;
        count.text = "2";
        yield return eof;
        count.text = "1";
        yield return eof;
        if (PhotonNetwork.IsMasterClient) {

            StartCoroutine(LoadingScreenProcess("InGame"));
     
        };
    }


    float timer = 0;
    IEnumerator LoadingScreenProcess(string sceneName){

        AsyncOperation ao = PhotonNetwork.LoadLevel("InGame");
        ao.allowSceneActivation = false;
        yield return null;

        
    }

    public override void OnJoinRandomFailed(short returnCode, string message){
        RoomOptions roomOpt = new RoomOptions();
        roomOpt.MaxPlayers = 2;
        roomOpt.IsVisible = true;
        roomOpt.IsOpen = true;
        this.roomName = roomMode + "_" + Random.Range(0,10);
        PhotonNetwork.CreateRoom(this.roomName, null);
    }
    private void OnChangeScene() => OnCompelet();

    public override void OnDisable() {
        NetworkObjectPool.instance.DestroyPool();
    }
}
