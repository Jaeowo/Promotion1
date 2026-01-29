using KBCore.Refs;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class PlayerController : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] private Rigidbody2D rb;
    [SerializeField, Self] private GroundChecker groundChecker;
    [SerializeField, Self] private Animator animator;
    [SerializeField, Self] private PlayerStat stats;
    [SerializeField, Anywhere] private InputReader input;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float jumpDuration = 0.3f;
    [SerializeField] private float jumpCooldown = 0f;
    [SerializeField] private float gravityMultiplier = 0.5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 100f;
    [SerializeField] private int maxDashCount = 1;
    [SerializeField] private int currentDashCount = 0;
    [SerializeField] private float dashCooldown = 30f;
    [SerializeField] private float dashDuration = 0.5f;

    [Header("Slash Settings")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] GameObject slashCollider;

    [Header("Death Settings")]
    [SerializeField] private bool isDeath = false;

    [Header("Hit Settings")]
    [SerializeField] private float hitDuration = 0.2f;
    [SerializeField] private float invicibleDuration = 2.0f;
    [SerializeField] private bool hitTriggered = false;

    private const float ZeroF = 0f;

    private float jumpVelocity;

    private enum ePlayerDirection
    {
        left,
        right,
    }
    private ePlayerDirection playerDirection = ePlayerDirection.left;

    [Header("Etc")]
    [SerializeField] private bool lockDirection = false;

    #region UNITY METHOD
    private void Awake()
    {
        SetUpTimer();
        SetUpStateMachine();
        SetUpEvent();
    }

    private void Start() => input.EnablePlayerActions();

    private void Update()
    {
        stateMachine.Update();

        CheckPlayerSide();
        CheckDeath();
        HandleTimers();

        // Tester
        if (Input.GetKeyDown(KeyCode.F))
        {
            stats.DecreaseCurrentHP(200);
        }
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
        var slashState = new SlashState(this, animator);
        var deathState = new DeathState(this, animator);
        var hitState = new HitState(this, animator);

        // Transition
        At(runState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
        At(idleState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));

        At(idleState, dashState, new FuncPredicate(() => dashTimer.IsRunning));
        At(dashState, idleState, new FuncPredicate(() => !dashTimer.IsRunning));

        At(idleState, slashState, new FuncPredicate(() => slashTimer.IsRunning));
        At(jumpState, slashState, new FuncPredicate(() => slashTimer.IsRunning));
        At(runState, slashState, new FuncPredicate(() => slashTimer.IsRunning));

        At(idleState, runState, new FuncPredicate(() => groundChecker.IsGrounded && Mathf.Abs(input.Direction.x) > 0.01f));

        // 무적상태가 아니면 어느 상태에서든 데미지 입는 상태로 변경하도록 수정하기
        //            OnHit?.Invoke();
        Any(deathState, new FuncPredicate(() => isDeath == true));
        Any(hitState, new FuncPredicate(() => hitTriggered == true));
        Any(idleState, new FuncPredicate(ReturnToIdleState));
  
        // 초기 상태
        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    // 모든 상태들이 러닝중이 아닐 때 idle상태로 진입하도록 하기
    bool ReturnToIdleState()
    {
        return groundChecker.IsGrounded
               && !jumpTimer.IsRunning
               && !dashTimer.IsRunning
               && !slashTimer.IsRunning 
               && Mathf.Abs(input.Direction.x) <= 0.01f;
    }

    #endregion

    #region TIMER
    List<Timer> timers;

    CountdownTimer jumpTimer;
    CountdownTimer jumpCooldownTimer;

    CountdownTimer dashTimer;
    CountdownTimer dashCooldownTimer;

    CountdownTimer slashTimer;

    StopwatchTimer idleBreakTimer;

    CountdownTimer invincibleTimer;
    CountdownTimer hitTimer;

    private void SetUpTimer()
    {
        SetUpIdleBreakTimer();
        SetUpInvicibleTimer();
        SetUpHitTimer();
        SetUpJumpTimer();
        SetUpDashTiemr();
        SetUpSlashTiemr();

        timers = new(8) { hitTimer, jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer, slashTimer, idleBreakTimer, invincibleTimer };
    }

    private void HandleTimers()
    {
        foreach (var timer in timers)
        {
            timer.Tick(Time.deltaTime);
        }
    }

    private void SetUpInvicibleTimer()
    {
        invincibleTimer = new CountdownTimer(invicibleDuration);
        invincibleTimer.OnTimerStop += () => SetInvincible(false);
    }

    private void SetUpHitTimer()
    {
        hitTimer = new CountdownTimer(hitDuration);
    }

    private void SetUpIdleBreakTimer()
    {
        idleBreakTimer = new StopwatchTimer();
    }

    private void SetUpSlashTiemr()
    {
        slashTimer = new CountdownTimer(attackCooldown);
    }

    private void SetUpJumpTimer()
    {
        jumpTimer = new CountdownTimer(jumpDuration);
        jumpCooldownTimer = new CountdownTimer(jumpCooldown);

        jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
    }

    private void SetUpDashTiemr()
    {
        dashTimer = new CountdownTimer(dashDuration);
        dashCooldownTimer = new CountdownTimer(dashCooldown);

        dashTimer.OnTimerStart += () =>
        {
            currentDashCount++; ;
        };
        dashTimer.OnTimerStop += () =>
        {
            if (currentDashCount > 0)
            {
                currentDashCount--;
            }

            dashCooldownTimer.Start();
        };
    }

    #endregion

    #region CHECK CONDITION
    private void OnEnable()
    {
        input.Jump += OnJump;
        input.Slash += OnSlash;
        input.Dash += OnDash;
        input.Skill += OnSkill;
    }

    private void OnDisable()
    {
        input.Jump -= OnJump;
        input.Slash -= OnSlash;
        input.Dash -= OnDash;
        input.Skill -= OnSkill;
    }

    private void OnSlash()
    {
        if(!slashTimer.IsRunning)
        {
            slashTimer.Start();
        }
    }

    private void OnSkill()
    {
  
    }

    private void OnJump()
    {
        if (!jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
        {
            jumpTimer.Start();
        }
        else if (jumpTimer.IsRunning)
        {
            jumpTimer.Stop();
        }
    }

    private void OnDash()
    {
        if (!dashTimer.IsRunning && !dashCooldownTimer.IsRunning && currentDashCount < maxDashCount)
        {
            dashTimer.Start();
        }
        else if(dashTimer.IsRunning)
        {
            dashTimer.Stop();
        }
    }

    private void CheckPlayerSide()
    {
        if(!lockDirection)
        {
            if (input.Direction.x > 0)
            {
                playerDirection = ePlayerDirection.right;
            }
            else if (input.Direction.x < 0)
            {
                playerDirection = ePlayerDirection.left;
            }

            if (playerDirection == ePlayerDirection.right)
            {
                transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }

    private void CheckDeath()
    {
        if (stats.CurrentHP <= 0)
        {
            isDeath = true;
        }
    }

    #endregion

    #region HANDLE STATE (Update)

    public void HandleSlash()
    {
        if (slashCollider && slashTimer.IsRunning)
        {
            slashCollider.SetActive(true);
        }
    }

    public bool HandleIdleBreakAnimation()
    {
        if (idleBreakTimer.GetTime() >= 3.0f)
        {
            idleBreakTimer.Reset();
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region HANDLE STATE (Fixed Update)

    public void HandleMovement()
    {
        rb.linearVelocity = new Vector2(input.Direction.x * moveSpeed , rb.linearVelocity.y);
    }

    public void HandleDash()
    {
        if(playerDirection == ePlayerDirection.right)
        {
            rb.linearVelocity = new Vector2(dashForce, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-dashForce, rb.linearVelocity.y);
        }
     
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

    #region ON ENTER

    public void EnterIdle()
    {
        idleBreakTimer.Reset();
        idleBreakTimer.Start();
    }

    public void EnterHit()
    {
        hitTimer.Reset();
        hitTimer.Start();
        invincibleTimer.Reset();
        invincibleTimer.Start();
        ClearTriggers();
        SetInvincible(true);
    }

    #endregion

    #region ON EXIT

    public void ExitSlash()
    {
        if(slashCollider)
        {
            slashCollider.SetActive(false);
        }
    }

    #endregion

    #region OTHER SETTING

    public void VelocityXZero()
    {
        var v = rb.linearVelocity;
        v.x = ZeroF;
        rb.linearVelocity = v;
    }

    public void LockPlayerDicrection(bool lockDirection)
    {
        this.lockDirection = lockDirection;
    }

    public void SetInvincible(bool isInvincible)
    {
        stats.ChangeInvencible(isInvincible);
    }

    public void TriggerHit()
    {
        hitTriggered = true;
    }

    public void ClearTriggers()
    {
        hitTriggered = false;
    }

    private void SetUpEvent()
    {
        stats.OnHit += () =>
        {
            TriggerHit();
        };
    }

    #endregion

}
