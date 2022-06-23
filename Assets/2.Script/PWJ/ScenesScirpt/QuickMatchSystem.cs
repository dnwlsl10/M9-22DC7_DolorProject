using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMatchSystem : DoorSystem
{
    [Header("UI")]
    private UIEarth uIEarth;

    public void Init(CockPit cockPit){
        
        this.uIEarth = cockPit.uIEarth;

    }
    private void Start(){
        InGame.OnQucikMatch += Enter;
        InGame.OnLobby += Exit;
    }

    public override void Enter(eRoomMode roomMode)
    {
        base.Open(() =>
        {
            this.uIEarth.Init();
        });
    }

    public void OnAction(){
        this.uIEarth.FindOtherPlayer(()=>{Exit();});
    }

    public override void Exit()
    {
        base.Close(() =>
        {
            this.uIEarth.Exit();
        });
    }

    private void OnDisable(){
        InGame.OnQucikMatch -= Enter;
        InGame.OnLobby -= Exit;
    }
}
