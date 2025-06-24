using UnityEngine;
using TMPro;

public class ShowPopup : MonoBehaviour
{
    [Header("Co się ma wyświetlić")]
    public GameObject popupPanel;         
    public TMP_Text popupText;            
    [TextArea]
    public string message;

    private void Start()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && popupPanel != null)
        {
            popupPanel.SetActive(true);
            if (popupText != null)
            {
                popupText.text = message;
                Debug.Log("Gracz wszedł w trigger!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }
}