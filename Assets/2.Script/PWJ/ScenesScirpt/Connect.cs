using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

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
    private bool bConnected;

    public System.Action OnCompelet;
    public System.Action<bool> IsMasterClient;
    private WaitForSeconds eof = new WaitForSeconds(3f);

    [Header("Count Text")]
    public GameObject[] count;

    public LoadingScreenProcess loadingScreenProcess;

    private string roomName;
    private readonly string gameVersion = "v1.0";
    private eRoomMode roomMode;

    public AudioClip[] connectBgms;
    public AudioClip onGameStartSFX;
    public AudioClip onCountSFX;

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
    public void CustomJoinedLobby(){
        Debug.Log("restart");
        bConnected = true;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(PhotonNetwork.InLobby);
        this.roomMode = eRoomMode.Lobby;

        if (bConnected) return;
        if (isTest)
        {
            robot = Instantiate(playerPrefab, target.transform.position, Quaternion.identity);
        }
        else{
            var prefab = Resources.Load<GameObject>("Prefab/" + robotData.connect_name);
            robot = Instantiate<GameObject>(prefab, target.transform.position, Quaternion.identity);
        }
        inGame.Init(robot, photonView);


        foreach (var bgm in connectBgms)
        {
            AudioPool.instance.Play(bgm.name, 1, this.transform.position, 0.5f);
        }
    }

    public void CustomCreatedRoom(eRoomMode room)
    {
        switch(room){
            case eRoomMode.Lobby: return;
            case eRoomMode.PracticeRoom: {
                    this.roomMode = room;
                    RoomOptions roomOpt = new RoomOptions();
                    roomOpt.MaxPlayers = 1;
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

    public override void OnLeftRoom(){}

    public override void OnCreatedRoom(){
        Debug.Log(roomMode);
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
    } 
      
    public override void OnJoinedRoom()
    {
        print(PhotonNetwork.CurrentLobby.Name + " " + PhotonNetwork.CurrentRoom.Name);
        if(this.roomMode == eRoomMode.QuickMatchRoom)  IsMasterClient(PhotonNetwork.IsMasterClient);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if(PhotonNetwork.IsMasterClient) inGame.DetectRemotePlayerJoin();

        FindUserBgmSFX();
    }

    public void FindUserBgmSFX(){
        photonView.RPC("GameStartRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void GameStartRPC(){
   
        StartCoroutine(OnStartCount());
    }
    IEnumerator OnStartCount()
    {
        yield return null;
        AudioPool.instance.Play(onGameStartSFX.name, 1, this.transform.position);

        for(int i =0 ; i< count.Length; i++){
            yield return eof;
            this.count[i].SetActive(true);
            AudioPool.instance.Play(onCountSFX.name, 2, this.transform.position);
            yield return eof;
            this.count[i].SetActive(false);
        }

        yield return StartCoroutine(loadingScreenProcess.LoadingPhotonScreenProcess(5, (ao) =>{
           ao.completed += (obj) =>{
               if (!isTest) OnCompelet();
            };
            PhotonNetwork._AsyncLevelLoadingOperation.allowSceneActivation = true;
        }));
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
}   