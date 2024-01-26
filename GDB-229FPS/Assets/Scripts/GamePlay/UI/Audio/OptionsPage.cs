using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPage : MonoBehaviour
{
    [SerializeField] Slider[] volSliders;
    [SerializeField] TMP_Text[] volText;
    [SerializeField] AudioClip changedVol;
    [SerializeField] Slider mouseSlider;
    [SerializeField] TMP_Text mouseText;
    [SerializeField] Toggle invertYToggle;

    [SerializeField] GameObject resetButton;

    bool playChangedVolClip;
    float[] vols = new float[3];
    int mouseSensitivity;
    int invertY;

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

    public void SetSensitivity(Slider val)
    {
        mouseSensitivity = (int)val.value;
        mouseText.text = (val.value - 300).ToString();
    }

    public void SetInvertY(Toggle toggle)
    {
        invertY = toggle.isOn ? 1 : 0;
    }

    public void SaveVolSettings()
    {
        PlayerPrefs.SetFloat("GameVol", vols[0]);
        PlayerPrefs.SetFloat("MusicVol", vols[1]);
        PlayerPrefs.SetFloat("SFXVol", vols[2]);
        PlayerPrefs.SetInt("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.SetInt("InvertY", invertY);
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

        mouseSensitivity = PlayerPrefs.GetInt("MouseSensitivity", 300);
        mouseSlider.value = mouseSensitivity;
        mouseText.text = (mouseSensitivity - 300).ToString();

        invertYToggle.isOn = PlayerPrefs.GetInt("InvertY", 0) == 1;

        resetButton.SetActive(PersistenceManager.instance.savedGameExists);

        playChangedVolClip = true;
    }

    public void ResetVolSettings()
    {
        PlayerPrefs.SetFloat("GameVol", 0.5f);
        PlayerPrefs.SetFloat("MusicVol", 0.5f);
        PlayerPrefs.SetFloat("SFXVol", 0.5f);
        PlayerPrefs.SetInt("MouseSensitivity", 300);
        PlayerPrefs.SetInt("InvertY", 0);
        LoadVolSettings();
    }

    public void DeleteGame()
    {
        PersistenceManager.instance.DeleteGame();
        resetButton.SetActive(false);
    }

}
