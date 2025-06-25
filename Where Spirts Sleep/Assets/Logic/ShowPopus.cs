using UnityEngine;
using TMPro;

public class ShowPopup : MonoBehaviour
{
    [Header("Co się ma wyświetlić")]
    public GameObject popupPanel;
    public TMP_Text popupText;

    [Header("Czy z przyciskiem?")]
    public bool keyShown;
    public KeyCode keyCode = KeyCode.E;

    [TextArea]
    public string message;

    private bool playerInRange = false;
    private bool messageShown = false;

    private void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    private void Update()
    {
        if (keyShown && playerInRange && !messageShown && Input.GetKeyDown(keyCode))
        {
            ShowMessage();
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
            if (popupPanel != null)
                popupPanel.SetActive(false);

            messageShown = false; // pozwala ponownie wyświetlić po wejściu
        }
    }

    private void ShowMessage()
    {
        if (popupPanel != null && popupText != null)
        {
            popupText.text = message;
            popupPanel.SetActive(true);
            messageShown = true;
            Debug.Log("Pokaż wiadomość");
        }
    }
}