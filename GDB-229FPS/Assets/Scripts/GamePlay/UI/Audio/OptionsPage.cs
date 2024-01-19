using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPage : MonoBehaviour
{
    [SerializeField] Slider[] volSliders;
    [SerializeField] TMP_Text[] volText;
    [SerializeField] AudioClip changedVol;

    bool playChangedVolClip;
    float[] vols = new float[3];

    private void Start()
    {
        LoadVolSettings();
    }

    public void SetGameVol(Slider vol)
    {
        AudioListener.volume = vol.value;
        vols[0] = vol.value;
        volText[0].text = (vol.value * 100).ToString("F0") + "%";
        if (playChangedVolClip)
        {
            AudioManager.instance.PlaySFX(changedVol);
        }
    }

    public void SetMusicVol(Slider vol)
    {
        AudioManager.instance.musicMixer.SetFloat("MusicVol", Mathf.Log10(vol.value) * 20);
        vols[1] = vol.value;
        volSliders[1].GetComponentInChildren<TMP_Text>().text = (vol.value * 100).ToString("F0") + "%";
    }
    public void SetSFXVol(Slider vol)
    {
        AudioManager.instance.sfxMixer.SetFloat("SFXVol", Mathf.Log10(vol.value) * 20);
        vols[2] = vol.value;
        volSliders[2].GetComponentInChildren<TMP_Text>().text = (vol.value * 100).ToString("F0") + "%";
        if (playChangedVolClip)
        {
            AudioManager.instance.PlaySFX(changedVol);
        }
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
        AudioManager.instance.musicMixer.SetFloat("MusicVol", Mathf.Log10(vols[1]) * 20);

        vols[2] = PlayerPrefs.GetFloat("SFXVol", 0.5f);
        AudioManager.instance.sfxMixer.SetFloat("SFXVol", Mathf.Log10(vols[2]) * 20);

        for (int i = 0; i < vols.Length && i < volSliders.Length; i++)
        {
            volSliders[i].value = vols[i];
            volSliders[i].GetComponentInChildren<TMP_Text>().text = (vols[i] * 100).ToString("F0") + "%";
        }

        playChangedVolClip = true;
    }

    public void ResetVolSettings()
    {
        PlayerPrefs.SetFloat("GameVol", 0.5f);
        PlayerPrefs.SetFloat("MusicVol", 0.5f);
        PlayerPrefs.SetFloat("SFXVol", 0.5f);
        LoadVolSettings();
    }

}
