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

    [SerializeField] private List<GameObject> players;
    private int playerCount = 0;

    private void Awake() 
    {
        print(PhotonNetwork.LevelLoadingProgress);
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        players = new List<GameObject>();
        photonView.RPC("Ready", RpcTarget.MasterClient);
        Transform spawn = spawnPoint[PhotonNetwork.IsMasterClient ? 0 : 1];
        myMech = PhotonNetwork.Instantiate(mechPrefab.name, spawn.position, spawn.rotation);
        Instantiate(networkObjectPool);
    }
    public void RegisterMech(GameObject mech) => players.Add(mech);

    [PunRPC]
    private void Ready()
    {
        if (PhotonNetwork.IsMasterClient)
            if (++playerCount == PhotonNetwork.CurrentRoom.PlayerCount)
                StartCoroutine(CheckBeforeStart());
    }

    IEnumerator CheckBeforeStart()
    {
        while(playerCount != players.Count) yield return null;
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
