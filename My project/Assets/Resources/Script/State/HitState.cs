using UnityEngine;

public class HitState : PlayerBaseState
{
    public HitState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("HitState Enter");
        animator.CrossFade(HitHash, crossFadeDuration);
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
