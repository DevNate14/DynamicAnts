using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] bool TeleportType;
    public bool Locked;
    public bool Open;
    [SerializeField] AudioClip[] OpenAudioClips;
    [SerializeField] AudioClip[] CloseAudioClips;
    [SerializeField] AudioSource Aud;
    public virtual void Interact()
    {

    }
    public virtual void PlayOpen()
    {
        if (OpenAudioClips.Length > 0)
            Aud.PlayOneShot(OpenAudioClips[Random.Range(0, OpenAudioClips.Length)], 1);
    }
    public virtual void PlayClose()
    {
        if (CloseAudioClips.Length > 0)
            Aud.PlayOneShot(CloseAudioClips[Random.Range(0, CloseAudioClips.Length)], 1);
    }
}
