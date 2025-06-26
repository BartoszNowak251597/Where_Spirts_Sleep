using UnityEngine;

namespace Logic
{
    
    public class AbilityPickup : MonoBehaviour
    {
        public PlayerAbility ability;
        public Sprite inactiveSprite;
        public Sprite activeSprite;
        
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.UnlockAbility(ability);
                if (_spriteRenderer != null && activeSprite != null)
                    _spriteRenderer.sprite = activeSprite;
                // Destroy(gameObject);
            }
        }
    }
}