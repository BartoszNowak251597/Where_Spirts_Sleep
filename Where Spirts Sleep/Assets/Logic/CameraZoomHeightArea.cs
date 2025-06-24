using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraZoomHeightArea : MonoBehaviour
    {
        public float targetSize = 15f;
        public float transitionSpeed = 2f;

        public float targetYOffset = 0f;
        public bool changeYOffset = false;

        private FollowCharacter followCam;
        private Camera mainCam;
        private bool playerInside = false;

        private float defaultSize;
        private float defaultYOffset;

        private void Start()
        {
            mainCam = Camera.main;
            if (mainCam != null)
            {
                defaultSize = mainCam.orthographicSize;
                followCam = mainCam.GetComponent<FollowCharacter>();
                if (followCam != null)
                {
                    defaultYOffset = followCam.cameraYOffset;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            playerInside = true;

            if (changeYOffset && followCam != null)
            {
                followCam.cameraYOffset = targetYOffset;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            playerInside = false;

            if (changeYOffset && followCam != null)
            {
                followCam.cameraYOffset = defaultYOffset;
            }
        }

        private void Update()
        {
            if (mainCam == null) return;

            float desiredSize = playerInside ? targetSize : defaultSize;
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, desiredSize, Time.deltaTime * transitionSpeed);
        }
    }
}