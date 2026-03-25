using UnityEngine;

public class PickupObject : MonoBehaviour
{
    private Rigidbody rb;
    private bool isHeld = false;
    private Transform holdPoint;

    public float rotateSpeed = 150f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isHeld)
        {
            transform.position = holdPoint.position;

            HandleRotation();
        }
    }

    void HandleRotation()
    {
        float rotX = 0f;
        float rotY = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
            rotX = -1f;
        if (Input.GetKey(KeyCode.RightArrow))
            rotX = 1f;

        if (Input.GetKey(KeyCode.UpArrow))
            rotY = 1f;
        if (Input.GetKey(KeyCode.DownArrow))
            rotY = -1f;

        transform.Rotate(Camera.main.transform.up, rotX * rotateSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Camera.main.transform.right, rotY * rotateSpeed * Time.deltaTime, Space.World);
    }

    public void Pickup(Transform holdTransform)
    {
        holdPoint = holdTransform;
        isHeld = true;

        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Drop()
    {
        isHeld = false;

        rb.useGravity = true;
        holdPoint = null;
    }
}