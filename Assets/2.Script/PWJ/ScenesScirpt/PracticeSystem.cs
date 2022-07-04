using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PracticeSystem : DoorSystem
{
  private CockPit cockPit;
    public void Init(CockPit cp){
        this.cockPit = cp;
    }
    public override void Enter(System.Action OnComplete)
    {
        cockPit.UnLockMode();
        base.Open(() =>{
            OnComplete();
            cockPit.EnterParticleMode();
      });
    }

    public override void Exit(System.Action OnComplete)
    {
        cockPit.LockMode();
        base.Close(()=>{
            OnComplete();
            cockPit.ExitParticleMode();
            
       });
    }

}
