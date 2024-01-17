using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] AudioSource musicSource;
    AudioMixer musicMixer;
    [SerializeField] AudioSource sfxSource;
    AudioMixer sfxMixer;

    [SerializeField] Slider[] volSliders;
    [SerializeField] AudioClip changedVol;
    [SerializeField] AudioClip[] backgroundMusics;

    bool playChangedVolClip;

    float[] vols = new float[3];

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(instance == null)
        { 
            instance = this;
        }
        musicMixer = musicSource.outputAudioMixerGroup.audioMixer;
        sfxMixer = sfxSource.outputAudioMixerGroup.audioMixer;

        LoadVolSettings();
    }

    public void SetGameVol(Slider vol)
    {
        AudioListener.volume = vol.value;
        vols[0] = vol.value;
        volSliders[0].GetComponentInChildren<TMP_Text>().text = (vol.value * 100).ToString("F0") + "%";
        PlaySFX();
    }

    public void SetMusicVol(Slider vol)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(vol.value) * 20);
        vols[1] = vol.value;
        volSliders[1].GetComponentInChildren<TMP_Text>().text = (vol.value * 100).ToString("F0") + "%";
    }
    public void SetSFXVol(Slider vol)
    {
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(vol.value) * 20);
        vols[2] = vol.value;
        volSliders[2].GetComponentInChildren<TMP_Text>().text = (vol.value * 100).ToString("F0") + "%";
        PlaySFX();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX()
    {
        if(!sfxSource.isPlaying && playChangedVolClip)
        {
            sfxSource.PlayOneShot(changedVol);
        }
    }

    public void PlayMusic(AudioClip music)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(vols[1]) * 20);
        musicSource.clip = music;
        musicSource.Play();
    }
    public void PlayMusic(int sceneNumber)
    {
        musicMixer.SetFloat("MusicVol", Mathf.Log10(vols[1]) * 20);
        musicSource.clip = backgroundMusics[sceneNumber];
        musicSource.Play();
    }

    public void SaveVolSettings()
    {
        PlayerPrefs.SetFloat("GameVol", vols[0]);
        PlayerPrefs.SetFloat("MusicVol", vols[1]);
        PlayerPrefs.SetFloat("SFXVol", vols[2]);
    }

    void LoadVolSettings()
    {
        playChangedVolClip = false;

        vols[0] = PlayerPrefs.GetFloat("GameVol", 0.5f);
        AudioListener.volume = vols[0];
        
        vols[1] = PlayerPrefs.GetFloat("MusicVol", 0.5f);
        musicMixer.SetFloat("MusicVol", Mathf.Log10(vols[1]) * 20);
        
        vols[2] = PlayerPrefs.GetFloat("SFXVol", 0.5f);
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(vols[2]) * 20);

        for(int i = 0; i < vols.Length && i < volSliders.Length; i++)
        {
            volSliders[i].value = vols[i];
            volSliders[i].GetComponentInChildren<TMP_Text>().text = (vols[i] * 100).ToString("F0") + "%";
        }

        playChangedVolClip = true;
    }

}
