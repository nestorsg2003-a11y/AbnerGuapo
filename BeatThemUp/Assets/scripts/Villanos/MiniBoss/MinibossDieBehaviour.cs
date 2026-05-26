using UnityEngine;

public class MinibossDieBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        MinibossControl.MinibossDied();

        Destroy(
            animator.GetComponentInParent<MinibossControl>()
            .gameObject);
    }
}