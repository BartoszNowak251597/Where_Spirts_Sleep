using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace Logic
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rb;
        public float walkingSpeed = 8f;
        public float crouchingSpeed = 10f;
        public float sprintingSpeed = 20f;
        public float jumpingHeight = 10f;
        public float crouchColliderScale = 0.5f;
        public Transform groundCheck;
        public LayerMask groundLayer;
        public float fallMultiplier = 10f;
        public Animator animator;
        public bool enableDoubleJumpManually = false;
        public BoxCollider2D playerCollider;
        public Transform ceilingCheck;
        public float ceilingCheckRadius = 0.2f;
        public ParticleSystem DirtParticle;

        private float _currentSpeed;
        private float _horizontalMovement;
        private int _jumpsLeft;
        private int _maxJumps;
        private HashSet<PlayerAbility> _unlockedAbilities = new();

        private bool _isGrounded;
        private bool _wasGrounded;
        private bool _isFacingRight;
        private bool _isSprinting;
        private bool _isCrouching;
        private bool _hasDoubleJump;
        private static PlayerMovement instance;

        private Vector2 _originalColliderSize;
        private Vector2 _originalColliderOffset;

        void Start()
        {
            _isFacingRight = transform.localScale.x > 0;
            _currentSpeed = walkingSpeed;

            _originalColliderSize = playerCollider.size;
            _originalColliderOffset = playerCollider.offset;

            _maxJumps = 1;
            _jumpsLeft = 1;

            if (enableDoubleJumpManually)
            {
                UnlockAbility(PlayerAbility.DoubleJump);
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            _isGrounded = Physics2D.OverlapCapsule(
                groundCheck.position, new Vector2(1.0f, 0.1f),
                CapsuleDirection2D.Horizontal, 0, groundLayer);

            if (_isGrounded && !_wasGrounded)
            {
                _jumpsLeft = _maxJumps;
            }
            _wasGrounded = _isGrounded;

            if (enableDoubleJumpManually && !_unlockedAbilities.Contains(PlayerAbility.DoubleJump))
            {
                UnlockAbility(PlayerAbility.DoubleJump);
            }

            Flip();
            rb.linearVelocity = new Vector2(_horizontalMovement * _currentSpeed, rb.linearVelocity.y);

            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
            
            if (!_isGrounded && _isSprinting)
            {
                _isSprinting = false;
                _currentSpeed = walkingSpeed;
            }
            
            float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
            if (horizontalSpeed < 0.1f)
            {
                horizontalSpeed = 0f;
                DirtParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
                

            animator.SetFloat("magnitude", horizontalSpeed);
            animator.SetBool("is_crouching", _isCrouching);
			animator.SetBool("is_grounded",_isGrounded);
            animator.SetBool("is_jumping", !_isGrounded && rb.linearVelocity.y > 0.1f);

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
            
            DirtParticle.Play();

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
                if (_isGrounded)
                {
                    _isSprinting = true;
                    _isCrouching = false;
                    _currentSpeed = sprintingSpeed;
                }
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

                float newHeight = _originalColliderSize.y * crouchColliderScale;
                float offsetAdjustment = (_originalColliderSize.y - newHeight) / 2f;

                playerCollider.size = new Vector2(_originalColliderSize.x, newHeight);
                playerCollider.offset = new Vector2(_originalColliderOffset.x, _originalColliderOffset.y - offsetAdjustment);
            }
            else if (context.canceled)
            {
                bool hasCeiling = Physics2D.OverlapCircle(ceilingCheck.position, ceilingCheckRadius, groundLayer);
    
                if (hasCeiling)
                {
                    return;
                }

                _isCrouching = false;
                _currentSpeed = walkingSpeed;

                playerCollider.size = _originalColliderSize;
                playerCollider.offset = _originalColliderOffset;
            }

        }


        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started && _jumpsLeft > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingHeight);
                _jumpsLeft--;
                animator.SetTrigger("Jump");
            }

            if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * Time.deltaTime);
            }
        }

        public void UnlockAbility(PlayerAbility ability)
        {
            if (_unlockedAbilities.Add(ability))
            {
                Debug.Log("Odblokowano umiejętność: " + ability);

                if (ability == PlayerAbility.DoubleJump)
                {
                    _hasDoubleJump = true;
                    animator.SetTrigger("Jump");
                }

                RefreshJumpSettings();
            }
            else
            {
                Debug.Log("Umiejętność już odblokowana: " + ability);
            }
        }

        public IEnumerable<PlayerAbility> GetUnlockedAbilities() => _unlockedAbilities;

        public bool HasDoubleJump() => _unlockedAbilities.Contains(PlayerAbility.DoubleJump);

        public void RefreshJumpSettings()
        {
            _maxJumps = HasDoubleJump() ? 2 : 1;
            _jumpsLeft = _isGrounded ? _maxJumps : Mathf.Min(_jumpsLeft, _maxJumps);
        }
    }
}
