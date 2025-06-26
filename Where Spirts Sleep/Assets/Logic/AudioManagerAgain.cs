using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerAgain : MonoBehaviour
{
    public static AudioManagerAgain Instance;

    public AudioSource musicSource;
    public List<AudioSource> ambientSources = new List<AudioSource>();
    public AudioSource sfxSource;
    private Coroutine musicFadeCoroutine;

    // Aktualny stan dźwięków
    private AudioClip currentMusic;
    private float currentMusicVolume = 1f;

    private Dictionary<int, (AudioClip clip, bool loop, float volume)> currentAmbients = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void PlayMusic(AudioClip clip, float volume = 1f, float fadeTime = 1f)
    {
        if (clip == null || musicSource == null) return;

        if (musicFadeCoroutine != null)
            StopCoroutine(musicFadeCoroutine);

        musicFadeCoroutine = StartCoroutine(FadeMusicRoutine(clip, volume, fadeTime));
    }

    public void SetMusicVolume(float volume)
    {
        currentMusicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
            musicSource.volume = currentMusicVolume;
    }

    public void PlayAmbient(AudioClip clip, int index = 0, bool loop = true, float volume = 1f)
    {
        if (clip == null || index >= ambientSources.Count) return;
        
        AudioSource ambient = ambientSources[index];
        
        if (ambient.isPlaying && ambient.clip == clip)
        {
            ambient.volume = Mathf.Clamp01(volume);
            return;
        }

        
        
        ambient.clip = clip;
        ambient.loop = loop;
        ambient.volume = volume;
        ambient.Play();
        
        currentAmbients[index] = (clip, loop, volume);
    }

    public void StopAmbient(int index)
    {
        if (index >= ambientSources.Count) return;

        ambientSources[index].Stop();
        if (currentAmbients.ContainsKey(index))
            currentAmbients.Remove(index);
    }
    
    public void PlaySFXLoop(AudioClip clip, float volume = 1f)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.loop = true;
        sfxSource.Play();
    }

    public void StopSFX()
    {
        if (sfxSource == null) return;
        sfxSource.Stop();
        sfxSource.clip = null;
    }

    
    public void FadeOutAmbient(int index, float duration = 1f)
    {
        if (index >= ambientSources.Count) return;
        StartCoroutine(FadeOut(ambientSources[index], duration));
    }
    
    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }
    
    public void SwapAmbientWithFade(AudioClip newClip, int index = 0, bool loop = true, float targetVolume = 1f, float fadeDuration = 1f)
    {
        if (index >= ambientSources.Count || newClip == null) return;
        StartCoroutine(SwapAmbientCoroutine(newClip, index, loop, targetVolume, fadeDuration));
    }
    
    private IEnumerator SwapAmbientCoroutine(AudioClip newClip, int index, bool loop, float targetVolume, float duration)
    {
        AudioSource source = ambientSources[index];

        // Fade out
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            yield return null;
        }

        source.Stop();

        // Nowy dźwięk
        source.clip = newClip;
        source.loop = loop;
        source.Play();

        // Fade in
        time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }
    
    private IEnumerator FadeMusicRoutine(AudioClip newClip, float targetVolume, float fadeTime)
    {
        // Fade out current music
        float startVolume = musicSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }
        musicSource.volume = 0f;

        // Zmień klip i zacznij odtwarzanie
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in do targetVolume
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeTime);
            yield return null;
        }
        musicSource.volume = targetVolume;
    }






}
