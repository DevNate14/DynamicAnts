using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager instance;

    List<IPersist> persistingObjects = new List<IPersist>();

    [SerializeField] List<GunStatsSO> allGunStatSO = new List<GunStatsSO>();

    public bool savedGameExists;

    public int sceneNumber;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        savedGameExists = 1 == PlayerPrefs.GetInt("SavedGameExists", 0);
        sceneNumber = PlayerPrefs.GetInt("SceneNumber", 1);
    }

    private void Update()
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
        savedGameExists = true;
        PlayerPrefs.SetInt("SavedGameExists", 1);
        PlayerPrefs.SetInt("SceneNumber", sceneNumber);

        foreach(IPersist p in persistingObjects)
        {
            p.SaveState();
        }
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
    }


    public List<GunStatsSO> LoadInventoryWeapons()
    {
        string weapons = PlayerPrefs.GetString("Weapons");

        List<GunStatsSO> loading = new List<GunStatsSO>();

        for(int i = 0; i < allGunStatSO.Count && i < weapons.Length; i++)
        {
            if (weapons[i] == '1')
            { 
                loading.Add(allGunStatSO[i]);
            }
        }

        return loading;
    }

    public void SaveInventoryWeapons(List<GunStatsSO> saving)
    {
        string weapons = "";

        for(int i = 0; i < allGunStatSO.Count; i++)
        {
            weapons += (saving.Contains(allGunStatSO[i]) ? "1" : "0");
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
