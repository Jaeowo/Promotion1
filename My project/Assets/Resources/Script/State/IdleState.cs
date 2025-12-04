using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(GameObject player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        animator.CrossFade(IdleHash, crossFadeDuration);
    }

    public override void Update()
    {

    }

    public override void OnExit()
    {

    }

}
