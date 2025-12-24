using UnityEngine;

public class DashState : BaseState
{
    public DashState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("DashState Enter");
        animator.CrossFade(DashHash, crossFadeDuration);

        player.LockPlayerDicrection(true);
        PlayerStatManager.instance.ChangeInvencible(true);
    }

    public override void FixedUpdate()
    {
        player.HandleDash();
    }

    public override void Update()
    {

    }

    public override void OnExit()
    {
        player.ExitVelocityXZero();
        player.LockPlayerDicrection(false);
        PlayerStatManager.instance.ChangeInvencible(false);
    }
}
