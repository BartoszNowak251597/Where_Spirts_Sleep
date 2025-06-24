using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraZoomArea : MonoBehaviour
    {
        public Transform focusPoint;
        public float targetSize = 15f;
        public float transitionSpeed = 2f;
        public float stayDuration = 3f;

        private FollowCharacter followCam;
        private Camera mainCam;

        private bool playerInside = false;
        private bool overrideActive = false;

        private float defaultSize;
        private float stayTimer = 0f;

        private void Start()
        {
            mainCam = Camera.main;
            if (mainCam != null)
            {
                defaultSize = mainCam.orthographicSize;
                followCam = mainCam.GetComponent<FollowCharacter>();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (followCam == null || focusPoint == null) return;

            playerInside = true;
            overrideActive = true;
            stayTimer = stayDuration;

            followCam.SetOverridePosition(new Vector3(focusPoint.position.x, focusPoint.position.y, -10f));
        }

        private void Update()
        {
            if (mainCam == null) return;

            // Obsługa zoomu
            float desiredSize = overrideActive ? targetSize : defaultSize;
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, desiredSize, Time.deltaTime * transitionSpeed);

            // Odliczanie czasu override
            if (overrideActive)
            {
                stayTimer -= Time.deltaTime;
                if (stayTimer <= 0f)
                {
                    followCam.ClearOverride();
                    overrideActive = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            playerInside = false;
            
            if (overrideActive)
            {
                followCam.ClearOverride();
                overrideActive = false;
            }
        }
    }
}
