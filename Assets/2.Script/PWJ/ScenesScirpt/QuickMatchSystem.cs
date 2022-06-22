using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMatchSystem : DoorSystem
{
    bool bQuickMatch;

    private void Start(){
        UIGame.OnPracticeMode += Init;
        UIGame.OnLobby += Exit;
    }

    public override void Init(eRoomMode roomMode)
    {
        base.Open(() =>
        {
            //퀵매치로직 
        });
    }

    public override void Exit()
    {
        base.Close(() =>
        {
            //퀵매치 종료 
        });
    }

    private void OnEnable(){
        UIGame.OnPracticeMode -= Init;
        UIGame.OnLobby -= Exit;
    }
}
