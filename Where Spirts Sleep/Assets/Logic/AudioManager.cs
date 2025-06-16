using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip background;

    private void Start()
    {
       musicSource.clip = background;
       musicSource.Play();
    }
    
}
