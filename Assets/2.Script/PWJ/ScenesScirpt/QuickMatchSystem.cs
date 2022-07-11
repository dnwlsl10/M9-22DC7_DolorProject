using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMatchSystem : DoorSystem
{
    [Header("UI")]
    private UIEarth uIEarth;
    private CockPit cockPit;
    private Animator prAni;

    public void Init(CockPit cp, Animator ani){
        this.cockPit = cp;
        this.prAni = ani;
    }

    public override void Enter(System.Action OnComplete)
    {
        cockPit.UnLockMode();
        base.Open(() =>
        {
            cockPit.EnterQuickMatchMode();
            OnComplete();
            prAni.SetTrigger("Start");
        });
    }

    public void OnFindOtherPlayer(){
        cockPit.FindOtherPlayer();
    } 

    // public override void Exit(System.Action OnComplete)
    // {
    //     this.uIEarth.Exit();
    //     base.Close(() =>
    //     {
    //         OnComplete();
    //     });
    // }
}
