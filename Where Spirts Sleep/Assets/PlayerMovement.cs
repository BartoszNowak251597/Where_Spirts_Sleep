using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float walkingSpeed = 20f;
    public float crouchingSpeed = 10f;
    public float sprintingSpeed = 30f;
    public float jumpingHeight = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float fallMultiplier = 5f;
    
    private float _currentSpeed;
    private float _horizontalMovement;
    private int _jumpsLeft;
    private int _maxJumps;
    
    private bool _isGrounded;
    private bool _wasGrounded;
    private bool _isFacingRight;
    private bool _isSprinting;
    private bool _isCrouching;
    private bool _hasDoubleJump;

    void Start()
    {
        _isFacingRight = transform.localScale.x > 0;
        _currentSpeed = walkingSpeed;

        _maxJumps = 1;
        _jumpsLeft = 1;
    }

    void Update()
    {
        _isGrounded = Physics2D.OverlapCapsule(
            groundCheck.position, new Vector2(1.8f, 0.2f),
            CapsuleDirection2D.Horizontal, 0, groundLayer);
        
        if (_isGrounded && !_wasGrounded)
        {
            _jumpsLeft = _maxJumps;
        }
        _wasGrounded = _isGrounded;

        Flip();

        rb.linearVelocity = new Vector2(_horizontalMovement * _currentSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
    }

    private void Flip()
    {
        if ((_isFacingRight && _horizontalMovement < 0) || (!_isFacingRight && _horizontalMovement > 0))
        {
            _isFacingRight = !_isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _horizontalMovement = context.ReadValue<Vector2>().x;
        
        if (_isCrouching)
            _currentSpeed = crouchingSpeed;
        else if (_isSprinting)
            _currentSpeed = sprintingSpeed;
        else
            _currentSpeed = walkingSpeed;
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isSprinting = true;
            _isCrouching = false;
            _currentSpeed = sprintingSpeed;
        }
        else if (context.canceled)
        {
            _isSprinting = false;
            _currentSpeed = walkingSpeed;
        }
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isCrouching = true;
            _isSprinting = false;
            _currentSpeed = crouchingSpeed;
        }
        else if (context.canceled)
        {
            _isCrouching = false;
            _currentSpeed = walkingSpeed;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && _jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingHeight);
            _jumpsLeft--;
        }
    }
    public void EnableDoubleJump()
    {
        if (!_hasDoubleJump)
        {
            _hasDoubleJump = true;
            _maxJumps = 2;

            if (_isGrounded)
            {
                _jumpsLeft = _maxJumps;
            }
        }
    }
}
