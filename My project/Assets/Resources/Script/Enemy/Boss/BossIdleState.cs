using UnityEngine;

public class BossIdleState : BossBaseState
{
    private float changeTime;

    public BossIdleState(Boss boss, Animator animator, float changeTime) : base(boss, animator)
    {
        this.changeTime = changeTime;
    }

    public override void OnEnter()
    {
        Debug.Log("EnemyIdle");
        animator.CrossFade(IdleHash, crossFadeDuration);
    }

    public override void Update()
    {   
    }

    public override void FixedUpdate()
    {
    }

    public override void OnExit()
    {
    }

}
