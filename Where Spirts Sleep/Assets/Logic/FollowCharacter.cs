using UnityEngine;

namespace Logic
{
    public class FollowCharacter : MonoBehaviour
    {
        public float followSpeed = 5f;
        public float height = 12f;
        public Transform target;

        private const float ZOffset = -10f;
        public bool lockX;
        public bool lockY;

        [Header("Limitowanie wysokości kamery")]
        public bool limitMaxY = false;
        public float maxY = 50f;

        [Header("Dynamiczny offset kamery")]
        public float cameraYOffset = 0f;

        private bool _overrideTarget = false;
        private Vector3 _overridePosition;

        private void Start()
        {
       
            cameraYOffset = height;
        }

        private void Update()
        {
            if (!_overrideTarget && target == null) return;

            Vector3 targetPos;
            if (_overrideTarget)
            {
                targetPos = new Vector3(
                    lockX ? transform.position.x : _overridePosition.x,
                    lockY ? transform.position.y : _overridePosition.y,
                    ZOffset
                );
            }
            else
            {
                float targetX = lockX ? transform.position.x : target.position.x;
                float targetY = lockY ? transform.position.y : target.position.y + cameraYOffset;

                if (limitMaxY)
                    targetY = Mathf.Min(targetY, maxY);

                targetPos = new Vector3(targetX, targetY, ZOffset);
            }

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        }

        public void SetOverridePosition(Vector3 position)
        {
            _overrideTarget = true;
            _overridePosition = position;
        }

        public void ClearOverride()
        {
            _overrideTarget = false;
        }
    }
}