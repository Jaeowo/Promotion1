using UnityEngine;

public class RunState : PlayerBaseState
{
    public RunState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("RunState Enter");
        animator.CrossFade(RunHash, crossFadeDuration);
    }

    public override void FixedUpdate()
    {
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
