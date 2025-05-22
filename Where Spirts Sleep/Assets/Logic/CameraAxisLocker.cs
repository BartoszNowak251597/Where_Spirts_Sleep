using UnityEngine;

namespace Logic
{
    public class CameraAxisLocker : MonoBehaviour
    {
        public bool lockX;
        public bool lockY;

        public bool affectX = true;
        public bool affectY = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Weszło w trigger");

            if (!other.CompareTag("Player")) {
                Debug.Log("To nie gracz");
                return;
            }

            Debug.Log("To jest gracz!");
            if (!Camera.main) {
                Debug.Log("Brak kamery!");
                return;
            }

            var cam = Camera.main.GetComponent<FollowCharacter>();
            if (!cam) {
                Debug.Log("Brak FollowCharacter na kamerze!");
                return;
            }

            Debug.Log("Zmiana blokady: X=" + lockX + ", Y=" + lockY);
            if (affectX) cam.lockX = lockX;
            if (affectY) cam.lockY = lockY;
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            Debug.Log("Gracz wyszedł z triggera");

            if (!Camera.main) return;
            var cam = Camera.main.GetComponent<FollowCharacter>();
            if (!cam) return;

            if (affectX) cam.lockX = false;
            if (affectY) cam.lockY = false;
        }
    }
}