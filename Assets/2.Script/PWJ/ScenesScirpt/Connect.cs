using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class Connect : MonoBehaviourPunCallbacks
{
    public System.Action OnCompelet;
    private UserInfo userInfo;
    public GameObject playerPrefab;
    public Transform target;
    private readonly string gameVersion = "v1.0";
    public bool isTest;

    /*    public InputField userIdTxt;
        public InputField roomNameTxt;

        private Dictionary<string, GameObject> roomDic = new Dictionary<string, GameObject>();

        public GameObject roomPrefab;

        //방들 부모 
        public Transform scrollContent;
    */

    public void Start()
    {
        if (isTest)
        {
            Init();
        }
    }

    public void Init(UserInfo userInfoData = null)
    {
        this.userInfo = userInfoData;
        PhotonNetwork.LogLevel = PunLogLevel.Full;
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 20;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("1. 포톤 서버에 접속");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("2. 로비에 접속");
        Instantiate(playerPrefab, target.transform.position , Quaternion.identity);
    }





  /*  public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤 룸 접속 실패");

        RoomOptions roomOpt = new RoomOptions();
        roomOpt.MaxPlayers = 4;
        roomOpt.IsVisible = true;
        roomOpt.IsOpen = true;

        roomNameTxt.text = "Room_" + Random.Range(1, 100);
        PhotonNetwork.JoinOrCreateRoom(roomNameTxt.text, roomOpt, null);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("3. 방 생성 완료");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("4. 방 입장 완료");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("InGame");
        }
    }

    //방리스트 




    //Random 버튼 클릭시
    public void OnRandom()
    {
        if (!string.IsNullOrEmpty(userIdTxt.text))
        {
            PhotonNetwork.NickName = userIdTxt.text;
            PhotonNetwork.JoinRandomRoom();
        }
    }

    //Room 버튼 클릭시 (룸생성)

    public void OnMakeRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;

        if (!string.IsNullOrEmpty(roomNameTxt.text))
        {
            PhotonNetwork.JoinOrCreateRoom(roomNameTxt.text, ro, null);
        }
    }

*/

   
    private void OnChangeScene()
    {
        OnCompelet();
    }
}

