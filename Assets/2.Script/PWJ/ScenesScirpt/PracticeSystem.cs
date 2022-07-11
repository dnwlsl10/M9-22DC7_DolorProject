using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeSystem : DoorSystem
{
  private CockPit cockPit;
  private Animator prAni;
    public void Init(CockPit cp, Animator ani){
        this.cockPit = cp;
        this.prAni = ani;
    }
    public override void Enter(System.Action OnComplete)
    {
        cockPit.UnLockMode();
        base.Open(() =>{
            OnComplete();
            cockPit.EnterParticleMode();
            prAni.SetTrigger("Start");
      });
    }

    public override void Exit(System.Action OnComplete)
    {
        cockPit.LockMode();
        base.Close(()=>{
            OnComplete();
            cockPit.ExitParticleMode();
            prAni.SetTrigger("Stop");
       });
    }

}
