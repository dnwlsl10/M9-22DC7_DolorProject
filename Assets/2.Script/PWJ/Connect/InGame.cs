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
    public UIPracice uIPracice;
    public UIQuickMatch uIQuickMatch;
    public UIExit uIExit;
    public Animator prAni;
    public void Init(GameObject obj, PhotonView pv)
    {
        this.pv = pv;
        var cokpit = obj.GetComponentInChildren<CockPit>();
        quickMatchSystem.Init(cokpit , prAni);
        practiceSystem.Init(cokpit, prAni);
        InitUI();
    }

    public void InitUI()
    {
        uIPracice.gameObject.transform.parent.gameObject.SetActive(true);
        uIQuickMatch.gameObject.transform.parent.gameObject.SetActive(true);
        
        uIPracice.OnSelceted = () =>{
            uIPracice.gameObject.transform.parent.gameObject.SetActive(false);
            uIQuickMatch.gameObject.transform.parent.gameObject.SetActive(false);
            EnterPracticeMode(() =>{
                uIExit.gameObject.transform.parent.gameObject.SetActive(true);
            });
        };

        uIQuickMatch.OnSelceted = () =>{
            uIPracice.gameObject.transform.parent.gameObject.SetActive(false);
            uIQuickMatch.gameObject.transform.parent.gameObject.SetActive(false);
            EnterQuickMatch(() =>{
            });
        };

        uIExit.OnSelceted =() =>{
            uIExit.gameObject.transform.parent.gameObject.SetActive(false);
            ExitPracticeMode(()=>{
                uIPracice.gameObject.transform.parent.gameObject.SetActive(true);
                uIQuickMatch.gameObject.transform.parent.gameObject.SetActive(true);
            });
        };
    }

    void EnterQuickMatch(System.Action OnComplete)
    {
        connect.CustomCreatedRoom(eRoomMode.QuickMatchRoom);

        connect.IsMasterClient = (isMasterClinet) =>
        {
            if (!isMasterClinet) return;
            quickMatchSystem.Enter(() =>{
                    OnComplete();
            });
        };
    }

    public void DetectRemotePlayerJoin(){
        quickMatchSystem.OnFindOtherPlayer();
        quickMatchSystem.Exit(() =>{});
    }
    

    void EnterPracticeMode(System.Action OnComplete)
    {
        connect.CustomCreatedRoom(eRoomMode.PracticeRoom);
        practiceSystem.Enter(()=>{ OnComplete();});
    }
    void ExitPracticeMode(System.Action OnComplete){
        connect.CustomJoinedLobby();
        practiceSystem.Exit(()=>{ OnComplete(); });
    }    
}
