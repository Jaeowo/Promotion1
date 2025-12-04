
using UnityEngine;

public abstract class BaseState : IState
{
    // 나중에 플레이어 컨트롤러 스크립트로 바꿔주기
    protected readonly GameObject player;
    protected readonly Animator animator;

    protected static readonly int IdleHash = Animator.StringToHash("Idle");
    protected static readonly int JumpHash = Animator.StringToHash("Jump");

    protected const float crossFadeDuration = 0.1f;

    protected BaseState(GameObject player, Animator animator)
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
    //public virtual void FixedUpdate()
    //{

    //}
    public virtual void OnExit()
    {

    }
}