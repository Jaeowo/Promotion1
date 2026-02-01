using UnityEngine;

public class EnemyBaseState : IState
{
    protected readonly Enemy enemy;
    protected readonly Animator animator;

    protected EnemyBaseState(Enemy enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }

    public void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    void IState.Update()
    {
    }
}
