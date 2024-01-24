using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PersistenceManager : MonoBehaviour
{

    public static PersistenceManager instance;

    List<IPersist> persistingObjects = new List<IPersist>();

    [SerializeField] List<GunStatsSO> allGunStatSO = new List<GunStatsSO>();
    [SerializeField] AudioClip saveSound;

    [Header("When testing individual levels: \ntestingLevel should be clicked and \nsaveGameExists should onl be clicked after \na checkpoint in the level has been activated")] //will need to be removed before beta build
    public bool testingLevel; //will need to be removed before beta build
    public bool savedGameExists;

    public int sceneNumber;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (!testingLevel) { savedGameExists = 1 == PlayerPrefs.GetInt("SavedGameExists", 0); }
        
        if(!savedGameExists)
        {
            DeleteGame();

            Vector3 defaultSpawn = GameObject.FindWithTag("Respawn").transform.position;
            if(defaultSpawn == null)
            {
                defaultSpawn = Vector3.zero;
            }    

            PlayerPrefs.SetFloat("SpawnPosX", defaultSpawn.x);
            PlayerPrefs.SetFloat("SpawnPosY", defaultSpawn.y);
            PlayerPrefs.SetFloat("SpawnPosZ", defaultSpawn.z);
        }

        sceneNumber = PlayerPrefs.GetInt("SceneNumber", SceneManager.GetActiveScene().buildIndex);
    }

    private void Update() //will need to be removed before beta build
    {
        if (Input.GetKeyDown(KeyCode.Backspace)) { DeleteGame(); print("Game Save Deleted"); }
        if (Input.GetKeyDown(KeyCode.Equals)) { SaveGame(); print("Game Saved"); }
    }

    public void AddToManager(IPersist adding)
    {
        persistingObjects.Add(adding); 
    }

    public void SaveGame()
    {
        GameManager.instance.saveMessage.SetActive(true);
        StartCoroutine(SavingMessage());
        savedGameExists = true;
        PlayerPrefs.SetInt("SavedGameExists", 1);
        PlayerPrefs.SetInt("SceneNumber", sceneNumber);

        foreach(IPersist p in persistingObjects)
        {
            p.SaveState();
        }
    }

    IEnumerator SavingMessage()
    {
        GameManager.instance.saveMessage.GetComponentInChildren<TMP_Text>().text = "Saving...";
        yield return new WaitForSeconds(1);
        StartCoroutine(GameSavedMessage());
    }
    IEnumerator GameSavedMessage()
    {
        AudioManager.instance.PlaySFX(saveSound);
        GameManager.instance.saveMessage.GetComponentInChildren<TMP_Text>().text = "Game Saved!";
        yield return new WaitForSeconds(3);
        GameManager.instance.saveMessage.SetActive(false);
    }

    public void DeleteGame()
    {
        float[] volSettings = { PlayerPrefs.GetFloat("GameVol"), PlayerPrefs.GetFloat("MusicVol"), PlayerPrefs.GetFloat("SFXVol") };
        
        PlayerPrefs.DeleteAll();
        savedGameExists = false;
        PlayerPrefs.SetInt("SavedGameExists", 0);

        PlayerPrefs.SetFloat("GameVol", volSettings[0]);
        PlayerPrefs.SetFloat("MusicVol", volSettings[1]);
        PlayerPrefs.SetFloat("SFXVol", volSettings[2]);

        PlayerPrefs.SetFloat("SpawnPosX", -262);
        PlayerPrefs.SetFloat("SpawnPosY", 10);
        PlayerPrefs.SetFloat("SpawnPosZ", 78);
    }


    public List<GunStatsSO> LoadInventoryWeapons()
    {
        string weapons = PlayerPrefs.GetString("Weapons");

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
