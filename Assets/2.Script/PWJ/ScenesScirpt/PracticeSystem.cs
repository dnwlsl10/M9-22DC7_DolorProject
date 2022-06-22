using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeSystem : DoorSystem
{
    private void Start() 
    {
        UIGame.OnPracticeMode += Init;
        UIGame.OnLobby += Exit;
    }
    public override void Init(eRoomMode eRoom)
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
        UIGame.OnPracticeMode -= Init;
        UIGame.OnLobby -= Exit;
    }
}
