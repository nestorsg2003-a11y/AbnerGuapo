using System.Collections;
using UnityEngine;

public class MinibossAttackState : EstadoFSM
{
    private Enemy myInfo;

    private float attackTimer;

    private float attackDuration = 1.5f;

    public MinibossAttackState(FSM fsm, Animator animator)
        : base(fsm, animator)
    {
        myInfo = animator.GetComponentInParent<Enemy>();
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("ATTACK STATE");

        // detener movimiento COMPLETAMENTE
        myInfo.GetComponent<Rigidbody>().velocity =
            Vector3.zero;

        // reiniciar timer
        attackTimer = 0f;

        // activar animación
        animator.ResetTrigger("dash");

        animator.ResetTrigger("move");

        animator.SetTrigger("attack");
    }

    public override void UpdateState()
    {
        // si murió
        if (myInfo.isDead)
        {
            fsm.ChangeState(
                (myInfo as MinibossControl).minibossDied);

            return;
        }

        // mantener quieto durante ataque
        myInfo.GetComponent<Rigidbody>().velocity =
            Vector3.zero;

        // contar tiempo
        attackTimer += Time.deltaTime;

        // terminar ataque
        if (attackTimer >= attackDuration)
        {
            fsm.ChangeState(
                (myInfo as MinibossControl).minibossIdle);

            return;
        }
    }

    public override void Exit()
    {
        myInfo.GetComponent<Rigidbody>().velocity =
            Vector3.zero;
    }
}