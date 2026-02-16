using UnityEngine;

public class BossBaseState : IState
{
    protected readonly Boss enemy;
    protected readonly Animator animator;

    protected const float crossFadeDuration = 0.1f;

    // 애니메이션 이름 해싱
    protected static readonly int IdleHash = Animator.StringToHash("Idle");
    protected static readonly int ActionHash = Animator.StringToHash("Action");
    protected static readonly int PrepLaserHash = Animator.StringToHash("Prep Laser");
    protected static readonly int LaserLoopHash = Animator.StringToHash("Laser Loop");
    protected static readonly int LaserEndHash = Animator.StringToHash("Laser End");
    protected static readonly int SummonOrbsHash = Animator.StringToHash("Summon Orbs");
    protected static readonly int HexSummonHash = Animator.StringToHash("Hex Summon?");
    protected static readonly int ElectricSmokeHash = Animator.StringToHash("Electric Smoke");
    protected static readonly int BuffRageHash = Animator.StringToHash("Buff/Rage");
    protected static readonly int DeathHash = Animator.StringToHash("Death");

    protected BossBaseState(Boss enemy, Animator animator)
    {
        this.enemy = enemy;
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
    }
}
