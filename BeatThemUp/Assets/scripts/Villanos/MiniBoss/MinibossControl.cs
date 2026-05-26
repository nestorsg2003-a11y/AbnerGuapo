using System;
using UnityEngine;

public class MinibossControl : Enemy
{
    public static event Action OnMinibossDead;

    public EstadoFSM minibossEnterState;
    public EstadoFSM minibossIdle;
    public EstadoFSM minibossAttack;
    public EstadoFSM minibossDied;
    public EstadoFSM minibossDash;

    private bool isActiveBoss = false;

    protected override void Start()
    {
        base.Start();

        minibossEnterState =
            new MinibossEnterState(enemyFSM, enemyAnimator);

        minibossIdle =
            new MinibossIdleState(enemyFSM, enemyAnimator);

        minibossAttack =
            new MinibossAttackState(enemyFSM, enemyAnimator);

        minibossDash =
            new MinibossDashState(enemyFSM, enemyAnimator);

        minibossDied =
            new MinibossDeadState(enemyFSM, enemyAnimator);

        MiniBossSensor.OnPlayerGetToMiniboss +=
            MinibossStartAction;
    }

    private void OnDestroy()
    {
        MiniBossSensor.OnPlayerGetToMiniboss -=
            MinibossStartAction;
    }

    public static void MinibossDied()
    {
        OnMinibossDead?.Invoke();
    }

    private void MinibossStartAction()
    {
        if (isActiveBoss) return;

        enemyAnimator.SetTrigger("enableBoss");

        enemyFSM.Init(minibossEnterState);

        isActiveBoss = true;
    }

    protected override void Update()
    {
        if (isActiveBoss)
            base.Update();
    }
}