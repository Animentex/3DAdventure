using UnityEngine;
using UnityEngine.InputSystem;

public class SkyGlideController : MonoBehaviour
{
    public float forwardSpeed = 25f;
    public float strafeSpeed = 10f;
    public float verticalSpeed = 8f;

    Rigidbody rb;

    PlayerInputActions input;

    Vector2 moveInput;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new PlayerInputActions();
    }


    void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };

        input.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
        };
    }


    void FixedUpdate()
    {
        Vector3 movement =
            transform.forward * forwardSpeed;

        movement +=
            transform.right *
            moveInput.x *
            strafeSpeed;

        movement +=
            transform.up *
            moveInput.y *
            verticalSpeed;


        rb.MovePosition(
            rb.position +
            movement * Time.fixedDeltaTime
        );
    }
}