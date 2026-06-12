using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed = 3.5f;
    [SerializeField] float runSpeed = 5.5f;
    [SerializeField] float sprintSpeed = 8f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -25f;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float acceleration = 12f;

    [Header("Crouch")]
    [SerializeField] float standingHeight = 2f;
    [SerializeField] float crouchHeight = 1f;

    [Header("References")]
    [SerializeField] Transform cameraTransform;

    CharacterController controller;

    PlayerInputActions input;

    Vector2 moveInput;
    Vector3 velocity;
    Vector3 currentMove;

    bool sprintHeld;
    bool crouched;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx =>
            moveInput = ctx.ReadValue<Vector2>();

        input.Player.Move.canceled += ctx =>
            moveInput = Vector2.zero;

        input.Player.Sprint.performed += _ =>
            sprintHeld = true;

        input.Player.Sprint.canceled += _ =>
            sprintHeld = false;

        input.Player.Jump.performed += _ =>
            Jump();

        input.Player.Crouch.performed += _ =>
            ToggleCrouch();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        Move();
        ApplyGravity();
    }

    void Move()
    {
        Vector3 inputDir =
            new Vector3(moveInput.x, 0, moveInput.y);

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection =
            camForward * inputDir.z +
            camRight * inputDir.x;

        float speed = 0f;

        if (moveInput.magnitude > 0.1f)
            speed = runSpeed;

        if (sprintHeld)
            speed = sprintSpeed;

        if (crouched)
            speed *= 0.5f;

        Vector3 targetMove =
            moveDirection.normalized * speed;

        currentMove = Vector3.Lerp(
            currentMove,
            targetMove,
            acceleration * Time.deltaTime);

        controller.Move(currentMove * Time.deltaTime);

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (!controller.isGrounded)
            return;

        velocity.y =
            Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void ToggleCrouch()
    {
        crouched = !crouched;

        controller.height =
            crouched
                ? crouchHeight
                : standingHeight;
    }
}