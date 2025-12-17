using KBCore.Refs;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] Rigidbody2D rb;
    [SerializeField, Self] GroundChecker groundChecker;
    [SerializeField, Self] Animator animator;
    [SerializeField, Anywhere] InputReader input;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Jump Settings")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpCooldown = 0f;
    [SerializeField] float gravityMultiplier = 1f;

    private const float Zerof = 0f;

    float velocity;
    float jumpVelocity;

    private Vector3 movement;

    List<Timer> timers;
    CountdownTimer jumpTimer;
    CountdownTimer jumpCooldownTimer;

    StateMachine stateMachine;

    private void Awake()
    {
        SetUpTimer();
        SetUpStateMachine();
    }

    private void SetUpTimer()
    {
        jumpTimer = new CountdownTimer(jumpDuration);
        jumpCooldownTimer = new CountdownTimer(jumpCooldown);
        timers = new List<Timer> { jumpTimer, jumpCooldownTimer };

        jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
    }

    private void SetUpStateMachine()
    {
        stateMachine = new StateMachine();

        // 상태 선언
        var idleState = new IdleState(this, animator);
        var jumpState = new JumpState(this, animator);

        // Transition
        At(idleState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
        At(jumpState, idleState, new FuncPredicate(() => groundChecker.IsGrounded && !jumpTimer.IsRunning));

        // 초기 상태
        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);



    private void Start() => input.EnablePlayerActions();

    private void OnEnable()
    {
        input.Jump += OnJump;
    }

    private void OnDisable()
    {
        input.Jump -= OnJump;
    }

    private void OnJump(bool performed)
    {
        if(performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
        {
            jumpTimer.Start();
        }
        else if(!performed && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }
    }

    private void Update()
    {
        movement = new Vector3(input.Direction.x, 0f, 0f);

        stateMachine.Update();

        UpdateAnimator();
        HandleTimers();

    }

    void FixedUpdate()
    {
        //HandleJump();
        //HandleMovement();
        stateMachine.FixedUpdate();
    }


    private void UpdateAnimator()
    {
        //animator.SetFloat(Speed, currentSpeed);
    }

    private void HandleTimers()
    {
        foreach (var timer in timers)
        {
            timer.Tick(Time.deltaTime);
        }
    }

    public void HandleMovement()
    {
        rb.linearVelocity = new Vector2(input.Direction.x * moveSpeed, rb.linearVelocity.y);
    }

    public void HandleJump()
    {
        if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
        {
            jumpVelocity = Zerof;
            return;
        }

        if (!jumpTimer.IsRunning)
        {
            jumpVelocity += Physics2D.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }


}
