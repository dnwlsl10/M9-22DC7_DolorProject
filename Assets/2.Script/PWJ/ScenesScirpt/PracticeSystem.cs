using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeSystem : DoorSystem
{
    public override void Enter()
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

}
