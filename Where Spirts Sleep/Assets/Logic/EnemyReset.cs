using UnityEngine;

namespace Logic
{
    public class EnemyReset : MonoBehaviour
    {
        private Vector3 _startPos;

        private void Awake()
        {
            _startPos = transform.position;
        }

        private void Start()
        {
            transform.position = _startPos;
        }
    }
}