using UnityEngine;

public class DoubleJumpPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player != null)
        {
            player.EnableDoubleJump();
            Destroy(gameObject);
        }
    }
}