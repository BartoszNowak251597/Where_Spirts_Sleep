using UnityEngine;

namespace Logic
{
    public class FollowCharacter : MonoBehaviour
    {
        public float followSpeed = 1000f;
        public float height = 12f;
        public Transform target;
        
        private const float ZOffset = -10f;
        public bool lockX;
        public bool lockY;

        private void Update()
        {
            if (target == null) return;

            var targetX = lockX ? transform.position.x : target.position.x;
            var targetY = lockY ? transform.position.y : target.position.y + height;
            var targetPosition = new Vector3(targetX, targetY, ZOffset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}