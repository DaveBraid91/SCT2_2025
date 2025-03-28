using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerController : MonoBehaviour, ITargeteableByAI
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
    [Header("Slide Stuff")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideSlope = 45;
    [SerializeField] private float slideSlowdownTime = 2;
    [SerializeField] private float slideRampUpFactor = 3;
    [SerializeField] private float slideRampDownFactor = 5;
    [SerializeField] private float slideFactorRampUp = 10;
    [SerializeField] private AnimationCurve slideSlowDownCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private CharacterController _characterController;
    private Animator _animator;

    private Vector3 _playerVelocity;
    private float _verticalVelocity;
    private Vector3 _slideVelocity;
    private float _slideVelocityFactor = 1;
    private float _slidingTime;
    private float _slidingSlowdownTimeInverse;
    
    private bool _isJumping = false;
    private bool _jumpEnded = true;
    private bool _isSliding = true;
    
    private static readonly int XSpeed = Animator.StringToHash("xSpeed");
    private static readonly int ZSpeed = Animator.StringToHash("zSpeed");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int EndJump = Animator.StringToHash("endJump");
    private static readonly int Crouched = Animator.StringToHash("crouched");

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        slideSlope = _characterController.slopeLimit;
        _slidingSlowdownTimeInverse = 1 / slideSlowdownTime;
    }

    private void Update()
    {
        UpdateMoveVelocity();
        UpdateVerticalVelocity();
        UpdateSlideVelocity();

        ApplyTotalVelocity();

        Crouch();
        UpdateRotation();
    }

    private void UpdateVerticalVelocity()
    {
        if (Input.GetAxisRaw("Jump") > 0.5f && _characterController.isGrounded && !_isJumping && !_isSliding)
        {
            _isJumping = true;
           _jumpEnded = false;
            _verticalVelocity = jumpForce;
            _animator.SetTrigger(Jump);
        }

        // if (_isJumping && !_characterController.isGrounded)
        // {
        //     _jumpEnded = true;
        // }

        if (_isJumping && _characterController.isGrounded && _characterController.velocity.y < 0)
        {
            _isJumping = false;
            _animator.SetTrigger(EndJump);
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
        var totalVelocity = _playerVelocity + _verticalVelocity * Vector3.up + _slideVelocity * _slideVelocityFactor;
        
        _characterController.Move(totalVelocity * Time.deltaTime);
    }

    private void UpdateMoveVelocity()
    {
        
        
        var xInput = /*!_isSliding ?*/ Input.GetAxis("Horizontal") /*: 0*/;
        var yInput = !_isSliding ? Input.GetAxis("Vertical") : 0;
        
        var input = cam.transform.right * xInput + cam.transform.forward * yInput;
        input.y = 0;
        
        if(input.sqrMagnitude > 1) input.Normalize();
        
        input = new Vector3(input.x * sideSpeed, 0, input.z * forwardSpeed);
        
        _playerVelocity = input;
        
        _animator.SetFloat(XSpeed, xInput);
        _animator.SetFloat(ZSpeed, yInput);
    }

    private void UpdateSlideVelocity()
    {
        var maxSlideVelocity = Vector3.zero;
        RaycastHit hit;
        if (_characterController.isGrounded && Physics.SphereCast(transform.position + _characterController.center,
                _characterController.radius, Vector3.down, out hit))
        {
            var angle = Vector3.Angle(hit.normal, Vector3.up);
            print(angle);

            if (angle > slideSlope)
            {
                _isSliding = true;
                
                var slideDirection = Vector3.ProjectOnPlane(hit.normal, Vector3.down).normalized;
                maxSlideVelocity = slideDirection * slideSpeed;
                
                Debug.DrawRay(hit.point, hit.normal, Color.red, 3f);
                Debug.DrawRay(hit.point, slideDirection, Color.blue, 3f);
            }
            else
            {
                _isSliding = false;
                _slidingTime = 0;
            }
            
            if(_isSliding)
                _slidingTime += Time.deltaTime;
            
            _slideVelocity = _isSliding
                ? Vector3.Lerp(_slideVelocity, maxSlideVelocity, Time.deltaTime * slideRampUpFactor)
                : Vector3.Lerp(_slideVelocity, Vector3.zero, Time.deltaTime * slideRampDownFactor);
            
            _slideVelocityFactor = _isSliding
                ? slideSlowDownCurve.Evaluate(Mathf.Clamp01(_slidingTime * _slidingSlowdownTimeInverse))
                : Mathf.Lerp(_slideVelocityFactor, 1, Time.deltaTime * slideFactorRampUp);
        }
    }

    private void Crouch()
    {
        //Here goes the logic of your exercise.
        _animator.SetBool(Crouched, Input.GetAxis("Crouch") > 0.5f);
    }

    public Transform GetTarget()
    {
        return transform;
    }
}
