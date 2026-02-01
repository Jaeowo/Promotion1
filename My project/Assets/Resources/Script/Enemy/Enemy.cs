using KBCore.Refs;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Child] Animator animator;

    StateMachine stateMachine;

    private void OnValidate() => this.ValidateRefs();

    private void Start()
    {
        stateMachine = new StateMachine();
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
} 
