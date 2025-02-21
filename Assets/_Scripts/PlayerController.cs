using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera cam;
    
    [Header("Movement")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float sideSpeed;
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;
    [Header ("Vertical Stuff")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float stickToGroundVelocity;

    private CharacterController _characterController;

    private Vector3 _playerVelocity;
    private float _verticalVelocity;
    
    private bool _isJumping = false;
    private bool _jumpEnded = true;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        UpdateMoveVelocity();
        UpdateVerticalVelocity();

        ApplyTotalVelocity();

        UpdateRotation();
    }

    private void UpdateVerticalVelocity()
    {
        if (Input.GetAxisRaw("Jump") > 0.5f && _characterController.isGrounded && !_isJumping)
        {
            _isJumping = true;
           _jumpEnded = false;
            _verticalVelocity = jumpForce;
        }

        // if (_isJumping && !_characterController.isGrounded)
        // {
        //     _jumpEnded = true;
        // }

        if (_isJumping && _characterController.isGrounded)
        {
            _isJumping = false;
        }

        if (_characterController.isGrounded && !_isJumping && _characterController.velocity.y < 0)
        {
            _verticalVelocity = -stickToGroundVelocity;
        }
        
        _verticalVelocity -= gravity * Time.deltaTime;
    }

    private void UpdateRotation()
    {
        var mouseXInput = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseXInput * rotationSpeed * Time.deltaTime, 0);
    }

    private void ApplyTotalVelocity()
    {
        var totalVelocity = _playerVelocity + _verticalVelocity * Vector3.up;
        
        _characterController.Move(totalVelocity * Time.deltaTime);
    }

    private void UpdateMoveVelocity()
    {
        var xInput = Input.GetAxis("Horizontal");
        var yInput = Input.GetAxis("Vertical");
        
        var input = cam.transform.right * xInput + cam.transform.forward * yInput;
        input.y = 0;
        
        if(input.sqrMagnitude > 1) input.Normalize();
        
        input = new Vector3(input.x * sideSpeed, 0, input.z * forwardSpeed);
        
        _playerVelocity = input;
    }
}
