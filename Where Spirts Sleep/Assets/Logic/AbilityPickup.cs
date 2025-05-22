using UnityEngine;

namespace Logic
{
    public class AbilityPickup : MonoBehaviour
    {
        public PlayerAbility ability;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.UnlockAbility(ability);
                Destroy(gameObject);
            }
        }
    }
}