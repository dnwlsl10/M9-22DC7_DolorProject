using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    /// <summary>
    /// 해당 상태를 시작 할때 1회 호출
    /// </summary>
    public abstract void Enter(Enemy entity);
    /// <summary>
    /// 해당 상태를 업데이트 할 때 프레임 호출 (실행하다)
    /// </summary>
    public abstract void Execute(Enemy entity);
    /// <summary>
    /// 해당 상태를 종료할 때 1회 호출 
    /// </summary>
    public abstract void Exit(Enemy entity);

}
