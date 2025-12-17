using KBCore.Refs;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] CharacterController controller;
    [SerializeField, Self] GroundChecker groundChecker;
    [SerializeField, Self] Animator animator;
    [SerializeField, Anywhere] InputReader input;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Jump Settings")]
    [SerializeField] float jumpDuration = 0.5f;
    [SerializeField] float jumpCooldown = 0f;
    [SerializeField] float gravityMultiplier = 3f;
    [SerializeField] float jumpMaxHeight = 0.1f;
    [SerializeField] float jumpForce = 2f;

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

        HandleMovement();
        HandleJump();


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
        controller.Move(movement * moveSpeed * Time.deltaTime);
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
            float launchPoint = 0.3f;

            if (jumpTimer.Progress > launchPoint)
            {
                jumpVelocity = Mathf.Sqrt(2f * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
            }
            else
            {
                jumpVelocity += (1f - jumpTimer.Progress) * jumpForce * Time.deltaTime;
            }
        }
        else
        {

            jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        controller.Move(Vector3.up * jumpVelocity * Time.deltaTime);
    }


}
