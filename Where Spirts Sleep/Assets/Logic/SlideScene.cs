using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlideScene : MonoBehaviour
{
    public Sprite[] storyslides;
    public Image displayImage;
    public AudioSource audioSource;
    public float slideDuration = 4f;
    public string nextScene = "1 tutorial area";

    private int currentIndex = 0;
    private float currentTime = 0f;
    private bool autoAdvance = false;

    void Start()
    {
        if (storyslides.Length > 0)
        {
            displayImage.sprite = storyslides[0];
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    void Update()
    {
        // ENTER or SPACE pressed?
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceSlide();
            return;
        }

        // Auto-advance after time
        if (autoAdvance)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= slideDuration)
            {
                currentTime = 0f;
                AdvanceSlide();
            }
        }
    }

    void AdvanceSlide()
    {
        currentIndex++;

        if (currentIndex < storyslides.Length)
        {
            displayImage.sprite = storyslides[currentIndex];
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}