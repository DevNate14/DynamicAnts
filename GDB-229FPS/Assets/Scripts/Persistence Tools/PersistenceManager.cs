using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PersistenceManager : MonoBehaviour
{

    public static PersistenceManager instance;

    List<IPersist> persistingObjects = new List<IPersist>();

    [SerializeField] List<GunStatsSO> allGunStatSO = new List<GunStatsSO>();
    [SerializeField] AudioClip saveSound;

    public bool savedGameExists;

    public int sceneNumber;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        savedGameExists = PlayerPrefs.GetInt("SavedGameExists", 0) == 1;

        sceneNumber = PlayerPrefs.GetInt("SceneNumber", SceneManager.GetActiveScene().buildIndex);
    }

    public void AddToManager(IPersist adding)
    {
        persistingObjects.Add(adding); 
    }

    public void SaveGame(bool playSound = true)
    {
        GameManager.instance.saveMessage.SetActive(true);
        StartCoroutine(SavingMessage(playSound));
        savedGameExists = true;
        PlayerPrefs.SetInt("SavedGameExists", 1);
        PlayerPrefs.SetInt("SceneNumber", sceneNumber);
        PlayerPrefs.GetInt("DamageDone", GameManager.instance.playerScript.damageDone);

        foreach (IPersist p in persistingObjects)
        {
            p.SaveState();
        }
    }

    IEnumerator SavingMessage(bool playSound)
    {
        GameManager.instance.saveMessage.GetComponentInChildren<TMP_Text>().text = "Saving...";
        yield return new WaitForSecondsRealtime(1);
        StartCoroutine(GameSavedMessage(playSound));
    }
    IEnumerator GameSavedMessage(bool playSound)
    {
        if (playSound) { AudioManager.instance.PlaySFX(saveSound); }
        GameManager.instance.saveMessage.GetComponentInChildren<TMP_Text>().text = "Game Saved!";
        yield return new WaitForSecondsRealtime(3);
        GameManager.instance.saveMessage.SetActive(false);
    }

    public void DeleteGame()
    {
        float[] volSettings = { PlayerPrefs.GetFloat("GameVol"), PlayerPrefs.GetFloat("MusicVol"), PlayerPrefs.GetFloat("SFXVol") };
        int mouseSensitivity = PlayerPrefs.GetInt("MouseSensitivity", 300);
        int invertY = PlayerPrefs.GetInt("InvertY", 0);
        int times = PlayerPrefs.GetInt("times", 0);

        PlayerPrefs.DeleteAll();
        savedGameExists = false;
        PlayerPrefs.SetInt("SavedGameExists", 0);

        PlayerPrefs.SetFloat("GameVol", volSettings[0]);
        PlayerPrefs.SetFloat("MusicVol", volSettings[1]);
        PlayerPrefs.SetFloat("SFXVol", volSettings[2]);
        PlayerPrefs.SetInt("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.SetInt("InvertY", invertY);
        PlayerPrefs.SetInt("times", times);

        PlayerPrefs.SetFloat("SpawnPosX", -262);
        PlayerPrefs.SetFloat("SpawnPosY", 10);
        PlayerPrefs.SetFloat("SpawnPosZ", 78);

        foreach (IPersist p in persistingObjects)
        {
            p.LoadState();
        }
    }


    public List<GunStatsSO> LoadInventoryWeapons()
    {
        string weapons = PlayerPrefs.GetString("Weapons", "");

        List<GunStatsSO> loading = new List<GunStatsSO>();

        for(int i = 0; i < weapons.Length; i++)
        {
            loading.Add(allGunStatSO[(int)char.GetNumericValue(weapons[i])]);
        }
        
        return loading;
    }

    public void SaveInventoryWeapons(List<GunStatsSO> saving)
    {
        string weapons = "";

        foreach (GunStatsSO weapon in saving)
        {
            for (int i = 0; i < allGunStatSO.Count; i++)
            {
                if(weapon == allGunStatSO[i])
                {
                    weapons += i.ToString();
                    break;
                }
            }
        }

        PlayerPrefs.SetString("Weapons", weapons);
    }

    public bool[] LoadInventoryKeys()
    {
        bool[] keys = new bool[3];

        string keyString = PlayerPrefs.GetString("Keys", "000");

        for(int i = 0; i < 3 && i < keyString.Length; i++)
        {
            keys[i] = keyString[i] == '1';
        }

        return keys;
    }

    public void SaveInventoryKeys(bool[] saving)
    {
        string keyString = "";

        for (int i = 0; i < saving.Length; i++)
        {
            keyString += saving[i] ? '1' : '0';
        } 

        PlayerPrefs.SetString("Keys", keyString);
    }

}
