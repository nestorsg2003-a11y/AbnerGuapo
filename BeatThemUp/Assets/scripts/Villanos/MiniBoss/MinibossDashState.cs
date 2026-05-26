using UnityEngine;

public class MinibossDashState : EstadoFSM
{
    private MinibossControl myInfo;

    private Vector3 dashDirection;

    private Vector3 targetPosition;

    private float dashTimer;

    private float maxDashTime = 3f;

    [SerializeField]
    private float dashSpeed = 12f;

    public MinibossDashState(FSM fsm, Animator animator)
        : base(fsm, animator)
    {
        myInfo =
            animator.GetComponentInParent<MinibossControl>();
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("DASH STATE");

        animator.SetTrigger("dash");

        targetPosition = myInfo.Player.position;

        // guardar direccion UNA sola vez
        dashDirection =
            (targetPosition - myInfo.transform.position).normalized;

        // ignorar altura
        dashDirection.y = 0f;

        dashTimer = 0f;

        // rotación estable
        if (dashDirection.x > 0.1f)
        {
            myInfo.transform.GetComponentInChildren<SpriteRenderer>()
                .transform.rotation =
                Quaternion.Euler(0f, 0f, 0f);
        }
        else if (dashDirection.x < -0.1f)
        {
            myInfo.transform.GetComponentInChildren<SpriteRenderer>()
                .transform.rotation =
                Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public override void UpdateState()
    {
        if (myInfo.isDead)
        {
            fsm.ChangeState(myInfo.minibossDied);

            return;
        }

        dashTimer += Time.deltaTime;

        // mover SIN recalcular dirección
        myInfo.GetComponent<Rigidbody>().velocity =
            dashDirection * dashSpeed;

        // llegó al objetivo
        if (Vector3.Distance(
            myInfo.transform.position,
            targetPosition) <= 1.2f)
        {
            myInfo.GetComponent<Rigidbody>().velocity =
                Vector3.zero;

            fsm.ChangeState(myInfo.minibossAttack);

            return;
        }

        // timeout dash
        if (dashTimer >= maxDashTime)
        {
            myInfo.GetComponent<Rigidbody>().velocity =
                Vector3.zero;

            fsm.ChangeState(myInfo.minibossIdle);

            return;
        }
    }

    public override void Exit()
    {
        myInfo.GetComponent<Rigidbody>().velocity =
            Vector3.zero;
    }
}