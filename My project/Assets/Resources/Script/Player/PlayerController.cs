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
    [SerializeField] float jumpMaxHeight = 1f;
    [SerializeField] float gravityMultiplier = 1f;

    private const float Zerof = 0f;

    float velocity;
    float jumpVelocity;

    private Vector3 movement;

    List<Timer> timers;
    CountdownTimer jumpTimer;
    CountdownTimer jumpCooldownTimer;

    private void Awake()
    {
        // 타이머 셋업
        jumpTimer = new CountdownTimer(jumpDuration);
        jumpCooldownTimer = new CountdownTimer(jumpCooldown);
        timers = new List<Timer> { jumpTimer, jumpCooldownTimer };

        jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
    }

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

        UpdateAnimator();

        HandleTimers();

    }

    void FixedUpdate()
    {
        HandleJump();
        HandleMovement();
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

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(input.Direction.x * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
        {
            jumpVelocity = Zerof;
            return;
        }

        if (jumpTimer.IsRunning)
        {
            float launchPoint = 0.9f;

            if (jumpTimer.Progress > launchPoint)
            {
                jumpVelocity = Mathf.Sqrt(2f * jumpMaxHeight * Mathf.Abs(Physics2D.gravity.y * rb.gravityScale) );
            }
            else
            {
                jumpVelocity += (1f - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
            }
        }
        else
        {
            jumpVelocity += Physics2D.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        }

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
    }


}
