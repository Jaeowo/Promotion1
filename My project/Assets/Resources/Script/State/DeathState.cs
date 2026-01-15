using UnityEngine;

public class DeathState : PlayerBaseState
{
    public DeathState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("DeathState Enter");
        animator.CrossFade(DeathHash, crossFadeDuration);
        player.LockPlayerDicrection(true);
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {

    }

    public override void OnExit()
    {
    }
}
