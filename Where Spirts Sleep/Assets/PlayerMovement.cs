using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float walkingSpeed;
    public float crouchingSpeed;
    public float sprintingSpeed;
    public float jumpingHeight;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float fallMultiplier;
    
    private float _currentSpeed;
    private float _horizontalMovement;
    
    private bool _isGrounded;
    private bool _isFacingRight;
    private bool _isSprinting;
    private bool _isCrouching;

    void Start()
    {
        _isFacingRight = transform.localScale.x > 0;
        _currentSpeed = walkingSpeed;
    }

    void Update()
    {
        _isGrounded = Physics2D.OverlapCapsule(
            groundCheck.position, new Vector2(1.8f, 0.2f),
            CapsuleDirection2D.Horizontal, 0, groundLayer);

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

        // Nie nadpisuj _currentSpeed — użyj flagi
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
        if (context.started && _isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingHeight);
        }
    }
}
