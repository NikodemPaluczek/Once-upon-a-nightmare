using UnityEngine;

public class PickableItem : MonoBehaviour, PickableObject
{
    private Rigidbody _rb;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private bool _isPicked;

    [SerializeField] private float _rotationSpeed = 150f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    public void OnPick(Transform holdPoint)
    {
        _isPicked = true;

        _rb.isKinematic = true;
        _rb.useGravity = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDrop()
    {
        _isPicked = false;

        transform.SetParent(null);

        transform.position = _startPosition;
        transform.rotation = _startRotation;

        _rb.isKinematic = false;
        _rb.useGravity = true;
    }

    public void Rotate(Vector2 input)
    {
        if (!_isPicked) return;

        float rotX = input.y * _rotationSpeed * Time.deltaTime;
        float rotY = -input.x * _rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, rotY, Space.World);
        transform.Rotate(Vector3.right, rotX, Space.World);
    }
}