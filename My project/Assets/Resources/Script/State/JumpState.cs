using UnityEngine;

public class JumpState : PlayerBaseState
{
    public JumpState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("JumpState Enter");
        animator.CrossFade(JumpHash, crossFadeDuration);
    }

    public override void FixedUpdate()
    {
        player.HandleJump();
        player.HandleMovement();
    }

    public override void Update()
    {

    }

    public override void OnExit()
    {
        player.VelocityXZero();
    }
}
