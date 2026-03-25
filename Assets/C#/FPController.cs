using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyFPS : MonoBehaviour
{
    public Transform cameraPivot;
    public InputReader input;

    public float speed = 6f;
    public float accel = 20f;
    public float mouseSensitivity = 120f;
    public float maxLook = 80f;

    Rigidbody rb;
    float xRot;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector2 move = input.Move;

        Vector3 dir =
            transform.right * move.x +
            transform.forward * move.y;

        Vector3 vel = rb.linearVelocity;
        Vector3 flatVel = new Vector3(vel.x, 0, vel.z);

        Vector3 target = dir * speed;
        Vector3 diff = target - flatVel;

        rb.AddForce(diff * accel, ForceMode.Acceleration);
    }

    void Look()
    {
        Vector2 look = input.Look * mouseSensitivity * Time.deltaTime;

        xRot -= look.y;
        xRot = Mathf.Clamp(xRot, -maxLook, maxLook);

        cameraPivot.localRotation = Quaternion.Euler(xRot, 0, 0);
        transform.Rotate(Vector3.up * look.x);
    }
}