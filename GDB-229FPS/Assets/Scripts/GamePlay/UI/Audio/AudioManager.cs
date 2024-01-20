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

    [SerializeField] AudioSource musicSource;
    public AudioMixer musicMixer;
    [SerializeField] AudioSource sfxSource;
    public AudioMixer sfxMixer;

    [SerializeField] AudioClip[] backgroundMusics;

    void Awake()
    {
        if(instance == null)
        { 
            instance = this;
        }

        //musicMixer = musicSource.outputAudioMixerGroup.audioMixer;
        //sfxMixer = sfxSource.outputAudioMixerGroup.audioMixer;

        AudioListener.volume = PlayerPrefs.GetFloat("GameVol", 0.25f);

        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 0.25f)) * 20);

        sfxMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVol", 0.25f)) * 20);
        //Should change Default to 25% -- 

        PlayMusic(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip music)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 0.5f)) * 20);
        musicSource.clip = music;
        musicSource.Play();
    }
    public void PlayMusic(int sceneNumber)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 0.5f)) * 20);
        musicSource.clip = backgroundMusics[sceneNumber];
        musicSource.Play();
    }
}
