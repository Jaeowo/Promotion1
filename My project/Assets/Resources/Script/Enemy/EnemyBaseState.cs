using UnityEngine;

public class EnemyBaseState : IState
{
    protected readonly BaseStat enemy;
    protected readonly Animator animator;

    // 애니메이션 이름 해싱
    //protected static readonly int IdleHash = Animator.StringToHash("Idle");


    protected const float crossFadeDuration = 0.1f;

    protected EnemyBaseState(BaseStat enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void OnEnter()
    {
    }
    public void Update()
    {
    }
    public void FixedUpdate()
    {
    }
    public void OnExit()
    {
    }


}
