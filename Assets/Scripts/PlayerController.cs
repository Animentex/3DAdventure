using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float runSpeed = 5.5f;
    [SerializeField] float sprintSpeed = 8f;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -25f;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float acceleration = 12f;
    [SerializeField] Transform modelTransform;

    [Header("Crouch")]
    [SerializeField] float standingHeight = 2f;
    [SerializeField] float crouchHeight = 1f;

    [Header("Free Fall")]
    [Header("Free Fall")]
    [SerializeField] float freeFallAirControl = 20f;
    [SerializeField] float diveGravityMultiplier = 2.5f;

    [SerializeField] float freeFallSpeedMultiplier = 1.2f;
    [SerializeField] float diveSpeedMultiplier = 0.5f;

    [SerializeField] float freeFallAngle = 90f;
    [SerializeField] float diveAngle = 180f;
    [SerializeField] float poseRotationSpeed = 8f;

    [Header("References")]
    [SerializeField] Transform cameraTransform;

    CharacterController controller;
    PlayerInputActions input;

    Vector2 moveInput;
    Vector3 velocity;
    Vector3 currentMove;

    bool sprintHeld;
    bool crouched;

    bool freeFalling;
    bool diving;

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

        input.Player.FreeFall.performed += _ =>
            EnterFreeFall();

        input.Player.Dive.performed += _ =>
        {
            if (freeFalling)
                diving = true;
        };

        input.Player.Dive.canceled += _ =>
    {
        diving = false;

        velocity.y *= 0.5f;
    };
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        Move();
        ApplyGravity();
        UpdateFallPose();
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
        {
            if (crouched)
            {
                crouched = false;
                controller.height = standingHeight;
            }

            speed = sprintSpeed;
        }

        if (crouched)
        speed *= 0.5f;

        // Slightly increased steering while free falling
        if (freeFalling)
        speed *= freeFallSpeedMultiplier;

        // Reduced steering while diving
        if (freeFalling && diving)
        speed *= diveSpeedMultiplier;

        Vector3 targetMove =
            moveDirection.normalized * speed;

        float moveAcceleration =
            freeFalling
                ? freeFallAirControl
                : acceleration;

        currentMove = Vector3.Lerp(
            currentMove,
            targetMove,
            moveAcceleration * Time.deltaTime);

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
    public void DisablePlayer()
    {
        enabled = false;
        currentMove = Vector3.zero;
        velocity = Vector3.zero;
    }

    public void EnablePlayer()
    {
        enabled = true;
    }

    void UpdateFallPose()
    {
    float targetPitch = 0f;

    if (freeFalling)
        targetPitch = freeFallAngle;

    if (diving)
        targetPitch = diveAngle;

    Quaternion targetRotation =
        Quaternion.Euler(targetPitch, 0f, 0f);

    modelTransform.localRotation =
        Quaternion.Slerp(
            modelTransform.localRotation,
            targetRotation,
            poseRotationSpeed * Time.deltaTime);
    }

    void EnterFreeFall()
    {
        if (controller.isGrounded)
            return;

        freeFalling = true;
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
        if (controller.isGrounded)
        {
            freeFalling = false;
            diving = false;

            if (velocity.y < 0)
                velocity.y = -2f;
        }

        float gravityMultiplier =
            (freeFalling && diving)
                ? diveGravityMultiplier
                : 1f;

        velocity.y +=
            gravity *
            gravityMultiplier *
            Time.deltaTime;

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