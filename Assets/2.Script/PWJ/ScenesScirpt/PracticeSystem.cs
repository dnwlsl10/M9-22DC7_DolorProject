using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeSystem : DoorSystem
{
    private void Start() 
    {
        InGame.OnPracticeMode += Enter;
        InGame.OnLobby += Exit;
    }
    public override void Enter(eRoomMode eRoom)
    {
        base.Open(() =>{
        //연습모드 로직 
      });
    }

    public override void Exit()
    {
        base.Close(()=>{
        //연습모드 종료 
       });
    }

    private void OnDisable(){
        InGame.OnPracticeMode -= Enter;
        InGame.OnLobby -= Exit;
    }
}
