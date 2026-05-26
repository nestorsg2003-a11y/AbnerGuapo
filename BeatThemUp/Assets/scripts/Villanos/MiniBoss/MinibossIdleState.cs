using System.Collections;
using UnityEngine;

public class MinibossIdleState : EstadoFSM
{
    private float idleTime;

    private Coroutine corutina;

    private bool finished;

    private Enemy myInfo;

    public MinibossIdleState(FSM fsm, Animator animator)
        : base(fsm, animator)
    {
        myInfo = animator.GetComponentInParent<Enemy>();
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("IDLE STATE");

        idleTime = Random.Range(3f, 5f);

        finished = false;

        corutina =
            fsm.mono.StartCoroutine(ActionCoroutine());

        myInfo.GetComponent<Rigidbody>().isKinematic = false;

        myInfo.EnableCollider();
    }

    public override void UpdateState()
    {
        if (myInfo.isDead)
        {
            fsm.ChangeState(
                (myInfo as MinibossControl).minibossDied);

            return;
        }

        if (myInfo.wasHitted)
        {
            myInfo.ResetHitted();

            animator.SetTrigger("damage");

            return;
        }

        if (finished)
        {
            fsm.ChangeState(
                (myInfo as MinibossControl).minibossDash);

            return;
        }
    }

    public override void Exit()
    {
        if (corutina != null)
            fsm.mono.StopCoroutine(corutina);
    }

    private IEnumerator ActionCoroutine()
    {
        yield return new WaitForSeconds(idleTime);

        finished = true;
    }
}