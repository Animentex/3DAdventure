using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    [Header("Camera")]
    public float distance = 5f;
    public float height = 2f;

    [Header("Rotation")]
    public float mouseSensitivity = 750f;

    [Header("Pitch Limits")]
    public float minPitch = -20f;
    public float maxPitch = 60f;

    [Header("Collision")]
    public LayerMask collisionMask;
    public float collisionRadius = 0.2f;
    public float collisionOffset = 0.1f;

    float yaw;
    float pitch = 15f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") *
               mouseSensitivity *
               Time.deltaTime;

        pitch -= Input.GetAxis("Mouse Y") *
                 mouseSensitivity *
                 Time.deltaTime;

        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation =
            Quaternion.Euler(pitch, yaw, 0);

        Vector3 pivotPosition =
            target.position + Vector3.up * height;

        Vector3 desiredPosition =
            pivotPosition
            - rotation * Vector3.forward * distance;

        Vector3 direction =
            (desiredPosition - pivotPosition).normalized;

        float targetDistance = distance;

        if (Physics.SphereCast(
                pivotPosition,
                collisionRadius,
                direction,
                out RaycastHit hit,
                distance,
                collisionMask))
        {
            targetDistance =
                Mathf.Max(
                    hit.distance - collisionOffset,
                    0.1f);
        }

        Vector3 finalPosition =
            pivotPosition +
            direction * targetDistance;

        transform.position = finalPosition;
        transform.rotation = rotation;
    }
}