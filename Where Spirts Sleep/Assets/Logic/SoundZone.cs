using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmbientSound
{
    public AudioClip clip;
    public int audioSourceIndex;
    public bool loop = true;

    [Range(0f, 1f)]
    public float volume = 1f;
}

public class SoundZone : MonoBehaviour
{
    [Header("Optional: Music")]
    public AudioClip newMusic;

    [Header("Ambients to play")]
    public List<AmbientSound> ambientClips = new List<AmbientSound>();

    [Header("Volume Overrides")]
    [Range(0f, 1f)] public float ambientVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;

    private static string lastZoneName = ""; // zapamiętaj ostatnią strefę

    private void Start()
    {
        // Jeśli gracz jest już w triggerze przy starcie, odpalić dźwięki
        Collider2D player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider2D>();
        Collider2D thisZone = GetComponent<Collider2D>();

        if (player != null && thisZone.bounds.Contains(player.bounds.center))
        {
            ApplyZoneSounds();
            lastZoneName = gameObject.name;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (lastZoneName == gameObject.name)
            return; // Jeśli to ta sama strefa, nic nie zmieniaj

        ApplyZoneSounds();
        lastZoneName = gameObject.name;
    }

    private void ApplyZoneSounds()
    {
        if (newMusic != null)
        {
            AudioManagerAgain.Instance.PlayMusic(newMusic, musicVolume, 2f);
        }
        
        // AudioManagerAgain.Instance.StopAllAmbients();

        List<int> usedIndices = new List<int>();
        
        foreach (var ambient in ambientClips)
        {
            usedIndices.Add(ambient.audioSourceIndex);

            AudioManagerAgain.Instance.SwapAmbientWithFade(
                ambient.clip,
                ambient.audioSourceIndex,
                ambient.loop,
                ambient.volume * ambientVolume,
                1f // fade 1 sekunda
            );
        }
        
        
        for (int i = 0; i < AudioManagerAgain.Instance.ambientSources.Count; i++)
        {
            if (!usedIndices.Contains(i))
            {
                AudioManagerAgain.Instance.FadeOutAmbient(i, 1f);
            }
        }
    }
}