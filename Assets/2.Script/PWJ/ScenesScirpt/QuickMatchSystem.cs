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

    public override void Enter()
    {
        this.uIEarth.Init();
        base.Open(() =>
        {
        });
    }

    public void OnFindOtherPlayer() => this.uIEarth.OnRaycast();

    public override void Exit()
    {
        this.uIEarth.Exit();
        base.Close(() =>
        {
        });
    }
}
