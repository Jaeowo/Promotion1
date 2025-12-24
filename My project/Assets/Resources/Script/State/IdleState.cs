using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("IdleState Enter");
        animator.CrossFade(IdleHash, crossFadeDuration);
        player.EnterIdle();
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        if(player.HandleIdleBreakAnimation())
        {
            animator.CrossFade(IdleBreakHash, crossFadeDuration);
        }

    }

    public override void OnExit()
    {

    }

}
