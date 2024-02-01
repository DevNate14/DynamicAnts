using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioMixer musicMixer;
    [SerializeField] AudioSource sfxSource;
    public AudioMixer sfxMixer;
    [SerializeField] AudioSource fadeSource;

    [SerializeField] AudioClip[] backgroundMusics;

    AudioClip musicQueue;

    float musicVolumeOrig;

    bool fading = false;

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

    private void Update()
    {
        if(!fading && musicQueue != null)
        {
            fadeSource.clip = musicQueue;
            fadeSource.volume = 0;
            fadeSource.Play();
            StartCoroutine(FadeMusic());
            musicQueue = null;
        }
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
            if (fading)
            {
                musicQueue = music;
                return;
            }
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
    public void PlayMusic(int sceneNumber, bool fade = true)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 0.5f)) * 20);
        if (fade)
        {
            if (fading)
            {
                musicQueue = backgroundMusics[sceneNumber];
                return;
            }
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

    IEnumerator FadeMusic()
    {
        AudioSource temp = fadeSource;

        while(musicSource.volume > 0.1 && fadeSource.volume < musicVolumeOrig - 0.1f)
        {
            fading = true;
            musicSource.volume -= 0.1f;
            fadeSource.volume += 0.1f;
            yield return new WaitForSecondsRealtime(0.5f);
        }

        fadeSource.volume = musicVolumeOrig;
        musicSource.volume = 0;

        fadeSource = musicSource;
        musicSource = temp;

        fadeSource.Stop();
        fading = false;
    }
}
