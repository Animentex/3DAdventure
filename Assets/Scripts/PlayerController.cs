using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float sprintSpeed = 6f;
    public float crouchSpeed = 1f;

    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    public CharacterController controller;

    private Vector3 velocity;
    private bool isGrounded;

    private bool isCrouching = false;
    private float originalHeight;
    public float crouchHeight = 1f;

    void Start()
    {
        if (!controller)
            controller = GetComponent<CharacterController>();

        originalHeight = controller.height;
    }

    void Update()
    {
        // Ground check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f; // Small downward force to stick to ground

        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Determine movement speed
        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftControl))       // Sprint when holding Left Control
        {
            speed = sprintSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))    // Run when holding Left Shift
        {
            speed = runSpeed;
        }

        if (isCrouching)
            speed = crouchSpeed;

        // Movement direction relative to player forward
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Crouch toggle
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                controller.height = crouchHeight;
            }
            else
            {
                controller.height = originalHeight;
            }
        }
    }
}
