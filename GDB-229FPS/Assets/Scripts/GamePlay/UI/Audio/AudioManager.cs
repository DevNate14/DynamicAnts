using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioMixer musicMixer;
    [SerializeField] AudioSource sfxSource;
    public AudioMixer sfxMixer;
    [SerializeField] AudioSource fadeSource;

    [SerializeField] AudioClip[] backgroundMusics;

    float musicVolumeOrig;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if(instance == null)
        { 
            instance = this;
        }

        //musicMixer = musicSource.outputAudioMixerGroup.audioMixer;
        //sfxMixer = sfxSource.outputAudioMixerGroup.audioMixer;

        AudioListener.volume = PlayerPrefs.GetFloat("GameVol", 0.5f);

        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 0.5f)) * 20);

        sfxMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVol", 0.5f)) * 20);

        musicVolumeOrig = musicSource.volume;
        
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip music, bool fade = true)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 0.5f)) * 20);
        if (fade)
        {
            fadeSource.clip = music;
            fadeSource.volume = 0;
            fadeSource.Play();
            StartCoroutine(FadeMusic());
        }
        else
        {
            musicSource.Stop();
            musicSource.clip = music;
            musicSource.Play();
        }
    }

    IEnumerator FadeMusic()
    {
        while(musicSource.volume > 0.1 && fadeSource.volume < musicVolumeOrig - 0.1f)
        {
            musicSource.volume -= 0.1f;
            fadeSource.volume += 0.1f;
            yield return new WaitForSeconds(0.5f);
        }

        fadeSource.volume = musicVolumeOrig;
        musicSource.volume = 0;

        AudioSource temp = fadeSource;
        fadeSource = musicSource;
        musicSource = temp;

        fadeSource.Stop();
    }

    public void PlayMusic(int sceneNumber, bool fade = true)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 0.5f)) * 20);
        if (fade)
        {
            fadeSource.clip = backgroundMusics[sceneNumber];
            fadeSource.volume = 0;
            fadeSource.Play();
            StartCoroutine(FadeMusic());
        }
        else
        {
            musicSource.Stop();
            musicSource.clip = backgroundMusics[sceneNumber];
            musicSource.Play();
        }
    }
}
