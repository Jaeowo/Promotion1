using UnityEngine;

public class DashState : BaseState
{
    public DashState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("DashState Enter");
        animator.CrossFade(DashHash, crossFadeDuration);
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

    }
}
