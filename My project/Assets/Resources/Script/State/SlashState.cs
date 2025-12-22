using UnityEngine;

public class SlashState : BaseState
{
    public SlashState(PlayerController player, Animator animator) : base(player, animator) { }

    public override void OnEnter()
    {
        Debug.Log("SlashState Enter");
        animator.CrossFade(SlashHash, crossFadeDuration);
    }

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        player.HandleSlash();
    }

    public override void OnExit()
    {
        player.ExitSlash();
    }
}
