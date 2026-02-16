using KBCore.Refs;
using UnityEngine;

[RequireComponent(typeof(PlayerDetector))]
public class Boss : MonoBehaviour
{
    [SerializeField, Self] PlayerDetector playerDetector;
    [SerializeField, Child] Animator animator;

    [SerializeField] private float wanderRadius = 10f;

    StateMachine stateMachine;

    private void OnValidate() => this.ValidateRefs();

    #region Unity Method

    private void Start()
    {
        stateMachine = new StateMachine();

        var idleState = new BossIdleState(this, animator, 5.0f);

        stateMachine.SetState(idleState);
    }
    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    #endregion
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

}
