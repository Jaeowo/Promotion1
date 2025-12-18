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
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float jumpDuration = 0.3f;
    [SerializeField] private float jumpCooldown = 0f;
    [SerializeField] private float gravityMultiplier = 0.5f;

    [Header("Dash Settings")]
    [SerializeField] private int maxDashCount = 1;
    [SerializeField] private int currentDashCount = 0;
    [SerializeField] private float dashCooldown = 30f;
    [SerializeField] private float dashDuration = 0.2f;

    private const float ZeroF = 0f;
    private Vector3 leftSide = new Vector3(0f, 180f, 0f);
    private Vector3 rightSide = new Vector3(0f, 0f, 0f);

    private float jumpVelocity;
    private float dashVelocity;


    #region UNITY METHOD
    private void Awake()
    {
        SetUpTimer();
        SetUpStateMachine();
    }

    private void Start() => input.EnablePlayerActions();

    private void Update()
    {
        stateMachine.Update();

        HandleTimers();
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    #endregion

    #region STATE MACHINE

    StateMachine stateMachine;
    private void SetUpStateMachine()
    {
        stateMachine = new StateMachine();

        // 상태 선언
        var idleState = new IdleState(this, animator);
        var jumpState = new JumpState(this, animator);
        var dashState = new DashState(this, animator);
        var runState = new RunState(this, animator);

        // Transition
        At(idleState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
        At(idleState, dashState, new FuncPredicate(() => dashTimer.IsRunning));
        //At(idleState, runState, new FuncPredicate(() => groundChecker.IsGrounded));
        Any(idleState, new FuncPredicate(ReturnToLocomotionState));

        // 초기 상태
        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    // 모든 상태들이 러닝중이 아닐 때 idle상태로 진입하도록 하기
    bool ReturnToLocomotionState()
    {
        return groundChecker.IsGrounded
               && !jumpTimer.IsRunning
               && !dashTimer.IsRunning;
    }


    #endregion

    #region TIMER
    List<Timer> timers;

    CountdownTimer jumpTimer;
    CountdownTimer jumpCooldownTimer;

    CountdownTimer dashTimer;
    CountdownTimer dashCooldownTimer;

    private void SetUpTimer()
    {
        // Jump
        jumpTimer = new CountdownTimer(jumpDuration);
        jumpCooldownTimer = new CountdownTimer(jumpCooldown);

        jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

        // Dash
        dashTimer = new CountdownTimer(dashDuration);
        dashCooldownTimer = new CountdownTimer(dashCooldown);

        dashTimer.OnTimerStart += () =>
        {
            dashVelocity = 2f;
            currentDashCount++; ;
        };
        dashTimer.OnTimerStop += () =>
        {
            if (currentDashCount >0)
            {
                currentDashCount--;
            }

            dashCooldownTimer.Start();
        };

        timers = new(4) { jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer };

    }

    private void HandleTimers()
    {
        foreach (var timer in timers)
        {
            timer.Tick(Time.deltaTime);
        }
    }
    #endregion

    #region CHECK CONDITION
    private void OnEnable()
    {
        input.Jump += OnJump;
        input.Dash += OnDash;
    }

    private void OnDisable()
    {
        input.Jump -= OnJump;
        input.Dash -= OnDash;
    }

    private void OnJump(bool performed)
    {
        if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
        {
            jumpTimer.Start();
        }
        else if (!performed && jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }
    }

    private void OnDash(bool performed)
    {
        if (performed && !dashTimer.IsRunning && !dashCooldownTimer.IsRunning && currentDashCount < maxDashCount)
        {
            dashTimer.Start();
        }
        else if(!performed && dashTimer.IsRunning)
        {
            dashTimer.Stop();
        }

    }

    #endregion

    #region HANDLE STATE
    public void HandleMovement()
    {
        rb.linearVelocity = new Vector2(input.Direction.x * moveSpeed , rb.linearVelocity.y);
    }

    public void HandleDash()
    {
        rb.linearVelocity = new Vector2( moveSpeed * dashVelocity, rb.linearVelocity.y);
    }

    public void HandleJump()
    {
        if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
        {
            jumpVelocity = ZeroF;
            return;
        }

        if (!jumpTimer.IsRunning)
        {
            jumpVelocity += Physics2D.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }
    #endregion

}
