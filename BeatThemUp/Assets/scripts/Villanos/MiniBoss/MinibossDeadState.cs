using UnityEngine;

public class MinibossDeadState : EstadoFSM
{
    private bool alreadyDead = false;

    public MinibossDeadState(FSM fsm, Animator animator)
        : base(fsm, animator)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (alreadyDead) return;

        alreadyDead = true;

        Debug.Log("MINIBOSS DEAD");

        animator.SetTrigger("dead");

        // LANZAR EVENTO DE VICTORIA
        MinibossControl.MinibossDied();

        // destruir boss después de unos segundos
        Object.Destroy(
            animator.transform.root.gameObject,
            3f);
    }

    public override void UpdateState()
    {
    }

    public override void Exit()
    {
    }
}