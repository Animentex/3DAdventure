using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Transform target;

    [Header("Camera")]
    [SerializeField] float distance = 5f;
    [SerializeField] float height = 2f;

    [Header("Rotation")]
    [SerializeField] float sensitivity = 0.15f;

    PlayerInputActions input;

    Vector2 lookInput;

    float yaw;
    float pitch = 15f;

    void Awake()
    {
        input = new PlayerInputActions();
    }

    void OnEnable()
    {
        input.Enable();

        input.Player.Look.performed += ctx =>
            lookInput = ctx.ReadValue<Vector2>();

        input.Player.Look.canceled += ctx =>
            lookInput = Vector2.zero;
    }

    void OnDisable()
    {
        input.Disable();
    }

    void LateUpdate()
    {
        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;

        pitch = Mathf.Clamp(pitch, -20f, 65f);

        Quaternion rotation =
            Quaternion.Euler(pitch, yaw, 0);

        Vector3 position =
            target.position
            - rotation * Vector3.forward * distance
            + Vector3.up * height;

        transform.position = position;
        transform.rotation = rotation;
    }
}