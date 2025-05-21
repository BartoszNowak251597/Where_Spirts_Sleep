using UnityEngine;

namespace Scenes
{
    public class FollowCharacter : MonoBehaviour
    {
        public float followSpeed = 1000f;
        public float height = 12f;
        private const float ZOffset = -10f;
        public Transform target;

        private void Update()
        {
            if (target == null) return;

            Vector3 targetPosition = new Vector3(target.position.x, target.position.y + height, ZOffset);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}