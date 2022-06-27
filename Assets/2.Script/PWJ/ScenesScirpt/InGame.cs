using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class InGame : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public QuickMatchSystem quickMatchSystem;
    public PracticeSystem practiceSystem;
    public Connect connect;
    private PhotonView pv;

    public void Init(GameObject obj, PhotonView pv)
    {
        this.pv = pv;
        this.connect = GameObject.FindObjectOfType<Connect>();
        var cokpit = obj.GetComponentInChildren<CockPit>();
        quickMatchSystem.Init(cokpit);
    }

    void EnterQuickMatch()
    {
        connect.CustomCreatedRoom(eRoomMode.QuickMatchRoom);

        connect.IsMasterClient = (isMasterClinet) =>
        {
            if (!isMasterClinet) return;
            quickMatchSystem.Enter();
        };
    }

    public void DetectRemotePlayerJoin(){
        quickMatchSystem.OnFindOtherPlayer();
        quickMatchSystem.Exit();
    }


    void EnterPracticeMode()
    {
        connect.CustomCreatedRoom(eRoomMode.PracticeRoom);
        practiceSystem.Enter();
    }
    void ExitPracticeMode(){
        practiceSystem.Exit();
        connect.OnJoinedLobby();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) EnterPracticeMode();
        if (Input.GetKeyDown(KeyCode.B)) EnterQuickMatch();
        if(Input.GetKeyDown(KeyCode.D)) ExitPracticeMode();
    }
}
