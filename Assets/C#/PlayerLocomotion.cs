using System;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    public static PlayerLocomotion Instance;
    //movement
    [SerializeField] private Transform _cameraObject;
    private Vector3 _moveDir;
    private Rigidbody _playerRB; 
    private float _moveSpeed = 4f;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }
    private void Awake()
    {
        _playerRB = GetComponent<Rigidbody>();
    }

    public void ManageMovement()
    {
        _moveDir = _cameraObject.forward * InputManager.Instance.VerticalInput;
        _moveDir = _moveDir + _cameraObject.right * InputManager.Instance.HorizontalInput;
        _moveDir.y = 0;
        _moveDir = _moveDir.normalized;

        Vector3 _moveVolocity = _moveDir * _moveSpeed;
        _playerRB.linearVelocity = _moveVolocity;
    }

    private void ManageRotation()
    {
        throw new NotImplementedException();
    }


}
