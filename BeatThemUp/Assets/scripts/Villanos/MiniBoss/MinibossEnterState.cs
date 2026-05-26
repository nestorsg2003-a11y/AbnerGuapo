using System.Collections;
using UnityEngine;

public class MinibossEnterState : EstadoFSM
{
    private float enterTime;

    private Coroutine corutina;

    private bool finished;

    private Enemy myInfo;

    public MinibossEnterState(FSM fsm, Animator animator)
        : base(fsm, animator)
    {
        myInfo = animator.GetComponentInParent<Enemy>();
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("ENTER STATE");

        enterTime = 2f;

        finished = false;

        corutina =
            fsm.mono.StartCoroutine(ActionCoroutine());

        myInfo.GetComponent<Rigidbody>().isKinematic = true;

        myInfo.DisableCollider();
    }

    public override void UpdateState()
    {
        if (finished)
        {
            fsm.ChangeState(
                (myInfo as MinibossControl).minibossIdle);

            return;
        }
    }

    public override void Exit()
    {
        if (corutina != null)
            fsm.mono.StopCoroutine(corutina);

        myInfo.GetComponent<Rigidbody>().isKinematic = false;

        myInfo.EnableCollider();
    }

    private IEnumerator ActionCoroutine()
    {
        yield return new WaitForSeconds(enterTime);

        finished = true;
    }
}