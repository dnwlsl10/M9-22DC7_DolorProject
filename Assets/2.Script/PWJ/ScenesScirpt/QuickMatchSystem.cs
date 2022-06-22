using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMatchSystem : DoorSystem
{
    [Header("UI")]
    public UIEarth earth;

    void Awake(){
        this.earth.gameObject.SetActive(false);
    }
    private void Start(){
        UIGame.OnQucikMatch += Init;
        UIGame.OnLobby += Exit;
    }

    public override void Init(eRoomMode roomMode)
    {
        this.earth.gameObject.SetActive(true);

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
            earth.gameObject.SetActive(false);
        });
    }

    private void OnEnable(){
        UIGame.OnPracticeMode -= Init;
        UIGame.OnLobby -= Exit;
    }
}
