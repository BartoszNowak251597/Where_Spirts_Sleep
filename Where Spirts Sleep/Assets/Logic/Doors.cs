using UnityEngine;

namespace Logic
{
    public class Door : MonoBehaviour
    {
        public int keysRequired = 3;
        private bool _playerInRange = false;

        void Update()
        {
            if (_playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                if (CollectibleManager.Instance.AllKeysCollected(keysRequired))
                {
                    OpenDoor();
                }
                else
                {
                    Debug.Log("Potrzebujesz więcej kluczy!");
                }
            }
        }

        private void OpenDoor()
        {
            Debug.Log("Drzwi otwarte!");
            // animacja dziwęki itd tutaj
            gameObject.SetActive(false);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _playerInRange = true;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _playerInRange = false;
        }
    }
}