using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("IdleState Enter");
        animator.CrossFade(IdleHash, crossFadeDuration);
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

    }

}
