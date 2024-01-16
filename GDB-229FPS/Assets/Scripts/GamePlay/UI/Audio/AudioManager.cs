using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public void SetGameVol(Slider vol)
    {
        AudioListener.volume = vol.value;
    }

    public void SetMusicVol(Slider vol)
    {
        musicSource.outputAudioMixerGroup.audioMixer.SetFloat("MusicVol", Mathf.Log10(vol.value) * 20);
    }
    public void SetSFXVol(Slider vol)
    {
        sfxSource.outputAudioMixerGroup.audioMixer.SetFloat("SFXVol", Mathf.Log10(vol.value) * 20);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

}
