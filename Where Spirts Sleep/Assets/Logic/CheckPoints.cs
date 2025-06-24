using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class Checkpoint : MonoBehaviour
    {
        public string checkpointID;
        
        public Sprite inactiveSprite;
        public Sprite activeSprite;
        
        private SpriteRenderer spriteRenderer;
        private bool isActive = false;


        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = inactiveSprite;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                SaveSystem.Save(
                    checkpointID,
                    SceneManager.GetActiveScene().buildIndex,
                    player.GetUnlockedAbilities(),
                    transform.position // pozycja checkpointu
                );
                
                spriteRenderer.sprite = activeSprite;
                isActive = true;


                Debug.Log("Checkpoint zapisany: " + checkpointID);
            }
        }
    }
}