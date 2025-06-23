using UnityEngine;
using System.Collections;

namespace Logic
{
    public class EnemyPatrol : MonoBehaviour
    {
        public Transform pointA;
        public Transform pointB;
        public float speed = 2f;
        public float waitTimeAtPoint = 2f;
        public Transform groundCheck;
        public LayerMask groundLayer;

        private Vector2 _target;
        private bool _isGrounded;
        private bool _facingRight = true;
        private bool _isWaiting = false;
        public Animator animator;

        private void Start()
        {
            _target = pointB.position;
        }

        private void Update()
        {
            if (_isWaiting)
                return;

            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            if (!_isGrounded)
                return;

            Vector2 newPos = Vector2.MoveTowards(transform.position, _target, speed * Time.deltaTime);
            transform.position = newPos;

            if (Vector2.Distance(transform.position, _target) < 6f && !_isWaiting)
            {
                StartCoroutine(SwitchDirectionAfterDelay());
            }
            
            float currentSpeed = _isWaiting ? 0f : speed;
            animator.SetFloat("speed", currentSpeed);
            animator.SetBool("is_waiting", _isWaiting);
        }

        private IEnumerator SwitchDirectionAfterDelay()
        {
            _isWaiting = true;
            yield return new WaitForSeconds(waitTimeAtPoint);

            if (Vector2.Distance(_target, pointA.position) < 6f)
            {
                _target = pointB.position;
            }
            else
            {
                _target = pointA.position;
            }

            Flip();
            _isWaiting = false;
        }


        private void Flip()
        {
            _facingRight = !_facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
            }
        }
#endif
    }
}