using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CameraZoomArea : MonoBehaviour
    {
        public Transform focusPoint;
        public float targetSize = 15f;
        public float transitionSpeed = 2f;

        private FollowCharacter followCam;
        private Camera mainCam;
        private bool playerInside = false;
        private float defaultSize;

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
            Debug.Log($"Trigger ENTER: {other.name}");

            if (!other.CompareTag("Player")) return;
            if (followCam == null) return;

            playerInside = true;
            if (focusPoint != null)
            {
                followCam.SetOverridePosition(new Vector3(focusPoint.position.x, focusPoint.position.y, -10f));
                Debug.Log($"Camera override to: {focusPoint.position}");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log($"Trigger EXIT: {other.name}");

            if (!other.CompareTag("Player")) return;
            if (followCam == null) return;

            playerInside = false;
            followCam.ClearOverride();
            Debug.Log("Camera returned to follow player.");
        }

        private void Update()
        {
            if (mainCam == null) return;

            float desiredSize = playerInside ? targetSize : defaultSize;
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, desiredSize, Time.deltaTime * transitionSpeed);
        }
    }
}