using UnityEngine;

public class NPCSound : MonoBehaviour
{
    public AudioClip talkSound;

    public void OnInteract()
    {
        AudioManagerAgain.Instance.PlaySFX(talkSound);
    }
}
