using UnityEngine;
using UnityEngine.SceneManagement;

public class StatueInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.F;
    public string sceneToLoad;

    private bool playerInRange = false;
    
    public Animator animator;


    private void Start()
    {
        if (animator == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                animator = player.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
            animator.SetBool("isreach", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
            animator.SetBool("isreach", false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}