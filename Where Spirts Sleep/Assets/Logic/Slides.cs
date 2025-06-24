using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;

namespace Logic
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Slides : MonoBehaviour
    {
        public Sprite[] storyslides;
        public TMP_Text SlideText;
        public GameObject backgroundPanel;
        private CanvasGroup panelCanvasGroup;
        public string[] SlideTexts;
        public Image displayImage;
        public AudioSource audioSource;
        public float slideDuration = 4f;
        public float fadeDuration = 1f;
        public string nextScene = "1 tutorial area";

        private int currentIndex = 0;
        private float timer = 0f;
        private bool isFading = false;
        private bool isFadingIn = false;
        private CanvasGroup canvasGroup;
        private CanvasGroup textCanvasGroup ;
        private CanvasGroup PanelCanvasGroup;
        

        void Start()
        {
            if (storyslides.Length > 0)
            {
                displayImage.sprite = storyslides[0];
            }

            if (SlideTexts.Length > 0 && SlideText != null)
            {
                SlideText.text = SlideTexts[0];
            }

            canvasGroup = displayImage.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1f;
            
            textCanvasGroup = SlideText.GetComponent<CanvasGroup>();
            textCanvasGroup.alpha = 1f;
            
            panelCanvasGroup = backgroundPanel.GetComponent<CanvasGroup>();
            panelCanvasGroup.alpha = 1f;
            
            
            

            if (audioSource != null)
                audioSource.Play();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                SkipToNextSlide();
            }

            if (isFading)
            {
                float fadeStep = Time.deltaTime / fadeDuration;
                if (isFadingIn)
                {
                    canvasGroup.alpha += fadeStep;
                    textCanvasGroup.alpha += fadeStep;
                    panelCanvasGroup.alpha += fadeStep;
                    if (canvasGroup.alpha >= 1f && textCanvasGroup.alpha >= 1f && panelCanvasGroup.alpha >= 1f)
                    {
                        canvasGroup.alpha = 1f;
                        textCanvasGroup.alpha = 1f;
                        panelCanvasGroup.alpha = 1f;
                        isFading = false;
                    }
                }
                else
                {
                    canvasGroup.alpha -= fadeStep;
                    textCanvasGroup.alpha -= fadeStep;
                    panelCanvasGroup.alpha -= fadeStep;
                    if (canvasGroup.alpha <= 0f && textCanvasGroup.alpha <= 0f && panelCanvasGroup.alpha <= 0f)
                    {
                        canvasGroup.alpha = 0f;
                        textCanvasGroup.alpha = 0f;
                        panelCanvasGroup.alpha = 0f;
                        SwitchToNextSlide();
                        isFadingIn = true;
                    }
                }

                return;
            }

            timer += Time.deltaTime;

            if (timer >= slideDuration)
            {
                timer = 0f;
                if (currentIndex + 1 < storyslides.Length)
                {
                    StartFadeOut();
                }
                else
                {
                    SceneManager.LoadScene(nextScene);
                }
            }
        }

        void SkipToNextSlide()
        {
            if (isFading) return;

            if (currentIndex + 1 < storyslides.Length)
            {
                timer = 0f;
                StartFadeOut();
            }
            else
            {
                SceneManager.LoadScene(nextScene);
            }
        }

        void StartFadeOut()
        {
            isFading = true;
            isFadingIn = false;
        }

        void SwitchToNextSlide()
        {
            currentIndex++;
            displayImage.sprite = storyslides[currentIndex];

            if (currentIndex < SlideTexts.Length && SlideText != null)
            {
                SlideText.text = SlideTexts[currentIndex];
            }
        }
    }
}