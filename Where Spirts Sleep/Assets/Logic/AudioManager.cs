using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    public AudioClip buttonClickSound;
    public AudioClip background;

    private void Start()
    {
       musicSource.clip = background;
       musicSource.Play();
    }
    
    public void PlayButtonClick()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound);
        }
    }
    
}
