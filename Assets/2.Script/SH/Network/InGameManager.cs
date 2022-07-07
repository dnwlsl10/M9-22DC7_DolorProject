using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class InGameManager : MonoBehaviourPunCallbacks
{
    public static InGameManager instance;

    [SerializeField] GameObject mechPrefab;
    [SerializeField] List<Transform> spawnPoint;
    [SerializeField] GameObject networkObjectPool;
    public GameObject myMech{get; private set;}

    [SerializeField] private List<GameObject> players = new List<GameObject>();
    private int playerCount = 0;
    public event System.Action onGameStart;
    public System.Action OnChangeLobby;

    [SerializeField] Robot selectPrefab;
    public void Init(UserInfo userInfo)
    {
        DataManager.GetInstance().LoadDatas();
        var selectPrefab = DataManager.GetInstance().dicRobotDatas[userInfo.userId];
    }

    public void Awake() 
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
        
        Transform spawn = spawnPoint[PhotonNetwork.IsMasterClient ? 0 : 1];

        myMech = PhotonNetwork.Instantiate(mechPrefab.name, spawn.position, spawn.rotation);
        Instantiate(networkObjectPool);
        
        photonView.RPC("Ready", RpcTarget.MasterClient);
    }
    public void RegisterMech(GameObject mech) => players.Add(mech);

    [PunRPC]
    private void Ready()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerCount++;
            StartCoroutine(CheckBeforeStart());
        }
    }

    IEnumerator CheckBeforeStart()
    {
        while(playerCount != players.Count || players.Count != PhotonNetwork.CurrentRoom.PlayerCount)
            yield return null;
            
        photonView.RPC("GameStart", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void GameStart() => StartCoroutine(StartCountDown());

    IEnumerator StartCountDown()
    {
        yield return new WaitForSecondsRealtime(3);

        foreach (var door in GameObject.FindObjectsOfType<DoorSystem>())
        {
            door.Open();
            StartCoroutine(Deactivate(door.transform.root.gameObject));
        }
        onGameStart?.Invoke();
    }

    IEnumerator Deactivate(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(3f);
        obj.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
}
