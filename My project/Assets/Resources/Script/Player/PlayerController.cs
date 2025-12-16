using KBCore.Refs;
using UnityEngine;

public class PlayerController : ValidatedMonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] CharacterController controller;
    [SerializeField, Self] Animator animator;
    [SerializeField, Anywhere] InputReader input;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 movement;

    private void Start()
    {
        //animator.Play("Idle");
    }

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        movement = new Vector3(input.Direction.x, 0f, 0f);
        controller.Move(movement * moveSpeed * Time.deltaTime);
        //animator.Play("Run");
    }

}
