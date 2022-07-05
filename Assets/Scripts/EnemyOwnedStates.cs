using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 소유 상태
/// </summary>
namespace EnemyOwnedStates 
{
    public class Idle : State //Idle 일때 실행되는 상태 
    {
        public override void Enter(Enemy entity) // 시작행동
        {
            entity.PrintText("시작");
            entity.HP = entity.maxHp;
        }

        public override void Execute(Enemy entity) // 지속행동
        {
            if(entity.HP > 0)
            {
                entity.HP -= 10;
                Debug.Log(entity.HP);
            }
            else
            {
                GameController.Stop(entity);
                return;
            }
        }

        public override void Exit(Enemy entity) // 종료 행동 
        {
            entity.PrintText("끝났당");
        }
    }

    public class Walk : State
    {
        public override void Enter(Enemy entity)
        {
            Debug.Log("걷는다");
        }

        public override void Execute(Enemy entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit(Enemy entity)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Run : State
    {
        public override void Enter(Enemy entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(Enemy entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit(Enemy entity)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Attack : State
    {
        public override void Enter(Enemy entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(Enemy entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit(Enemy entity)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Shout : State
    {
        public override void Enter(Enemy entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Execute(Enemy entity)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit(Enemy entity)
        {
            throw new System.NotImplementedException();
        }
    }

}


