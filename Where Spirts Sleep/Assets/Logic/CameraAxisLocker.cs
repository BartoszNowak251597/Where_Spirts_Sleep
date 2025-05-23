using UnityEngine;

namespace Logic
{
    public class CameraAxisLocker : MonoBehaviour
    {
        public bool lockX;
        public bool lockY;

        public bool affectX = true;
        public bool affectY = true;

        private bool _playerInside;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInside = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInside = false;

                if (Camera.main)
                {
                    var cam = Camera.main.GetComponent<FollowCharacter>();
                    if (cam != null)
                    {
                        if (affectX) cam.lockX = false;
                        if (affectY) cam.lockY = false;
                    }
                }
            }
        }

        private void Update()
        {
            if (_playerInside && Camera.main)
            {
                var cam = Camera.main.GetComponent<FollowCharacter>();
                if (cam != null)
                {
                    if (affectX) cam.lockX = lockX;
                    if (affectY) cam.lockY = lockY;
                }
            }
        }
    }
}