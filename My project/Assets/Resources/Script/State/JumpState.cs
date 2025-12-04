using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(GameObject player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        animator.CrossFade(JumpHash, crossFadeDuration);
    }

    public override void Update()
    {

    }

    public override void OnExit()
    {

    }
}
