
using UnityEngine;

public abstract class BaseState : IState
{
    protected readonly PlayerController player;
    protected readonly Animator animator;

    // 애니메이션 이름 해싱
    protected static readonly int IdleHash = Animator.StringToHash("Idle");
    protected static readonly int JumpHash = Animator.StringToHash("Jump");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int DashHash = Animator.StringToHash("Dash");
    protected static readonly int SlashHash = Animator.StringToHash("Slash");
    protected static readonly int SkillHash = Animator.StringToHash("SpecialSkill");

    protected const float crossFadeDuration = 0.1f;

    protected BaseState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

    public virtual void OnEnter()
    {

    }
    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }
    public virtual void OnExit()
    {
        Debug.Log("BaseState Exit");
    }
}