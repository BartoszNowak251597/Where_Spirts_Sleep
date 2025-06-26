using UnityEngine;

public class NPCSound : MonoBehaviour
{
    public AudioClip talkSound;
    private bool playerInRange = false;
    private AudioSource localAudioSource;

    private void Awake()
    {
        // Pobierz AudioSource z tego obiektu
        localAudioSource = GetComponent<AudioSource>();
        if (localAudioSource == null)
        {
            Debug.LogError("Brak AudioSource na NPC!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            StopTalkSound();
        }
    }

    private void Update()
    {
        if (!playerInRange || localAudioSource == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayTalkSound();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StopTalkSound();
        }
    }

    private void PlayTalkSound()
    {
        if (!localAudioSource.isPlaying)
        {
            localAudioSource.clip = talkSound;
            localAudioSource.Play();
        }
    }

    private void StopTalkSound()
    {
        if (localAudioSource.isPlaying)
        {
            localAudioSource.Stop();
        }
    }
}