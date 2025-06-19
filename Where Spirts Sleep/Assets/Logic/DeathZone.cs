using UnityEngine;
using UnityEngine.SceneManagement;

namespace Logic
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DeathZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Coś weszło do DeathZone: " + other.name);

            if (other.CompareTag("Player"))
            {
                Debug.Log("Gracz zginął – restart sceny!");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}