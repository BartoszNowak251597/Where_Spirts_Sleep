using UnityEngine;

namespace Logic
{
    public class Collectible : MonoBehaviour
    {
        public CollectibleID collectibleID;
        private bool playerInRange = false;

        void Update()
        {
            if (playerInRange && Input.GetKeyDown(KeyCode.E))
            {
                if (CollectibleManager.Instance.CanCollect(collectibleID))
                {
                    CollectibleManager.Instance.Collect(collectibleID);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Nie możesz tego jeszcze zebrać!");
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                playerInRange = true;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                playerInRange = false;
        }
    }
}