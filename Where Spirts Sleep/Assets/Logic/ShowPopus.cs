using UnityEngine;
using TMPro;

public class ShowPopup : MonoBehaviour
{
    [Header("Co się ma wyświetlić")]
    public GameObject popupPanel;
    public TMP_Text popupText;

    [Header("Czy z przyciskiem?")]
    public bool keyShown;
    public KeyCode openKey = KeyCode.E;
    public KeyCode closeKey = KeyCode.Q;

    [TextArea]
    public string message;

    private bool playerInRange = false;
    private bool messageShown = false;
    
    public Animator animator;

    private void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
        
        if (animator == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                animator = player.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (keyShown && !messageShown && Input.GetKeyDown(openKey))
            {
                if (animator != null)
                {
                    animator.SetBool("IsMessageShown", true);
                }
                ShowMessage();
                
            }

            if (messageShown && Input.GetKeyDown(closeKey))
            {
                if (animator != null)
                {
                    animator.SetBool("IsMessageShown", false);
                }
                HideMessage();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!keyShown)
                ShowMessage();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideMessage(); // chowa automatycznie po odejściu
        }
    }

    private void ShowMessage()
    {
        if (popupPanel != null && popupText != null)
        {
            popupText.text = message;
            popupPanel.SetActive(true);
            messageShown = true;
            
            Debug.Log("🗨️ Pokazano wiadomość");
        }
    }

    private void HideMessage()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);

        

        messageShown = false;
        Debug.Log("❌ Ukryto wiadomość");
    }
}