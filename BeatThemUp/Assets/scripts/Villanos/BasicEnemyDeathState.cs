using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyDeathState : EstadoFSM
{
    public BasicEnemyDeathState(FSM fsm, Animator animator) : base(fsm, animator)
    {
    }

    public override void Enter()
    {
        animator.SetTrigger("die");
    }

    public override void Exit()
    {
    }

    public override void UpdateState()
    {
    }
}