using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory, IPersist
{
    [SerializeField] List<GunStatsSO> weapons = new List<GunStatsSO>();
    //[SerializeField] UpgradeItem[] items;
    [SerializeField] AudioClip reloadClip;
    int selectedWeapon;

    private void Start()
    {
        AddToPersistenceManager();
        LoadState();
    }
    private void OnDestroy()
    {
        AddToPersistenceManager();
    }
    private void Update() {
        if (!GameManager.instance.isPaused && weapons.Count > 0) {
            if (!weapons[selectedWeapon].isShooting) {
                if (Input.GetButton("Shoot")) {
                    ShootGun();
                }
                if (Input.GetButton("Reload")) {
                    ReloadGun();
                }
                SelectGun();
            }
        }
    }
    public void PickUpWeapon(GunStatsSO stats) {
        string wepName = stats.name;
        if (weapons.Count != 0)
        {
            if (stats.ammoCount != -1)
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    if (wepName == weapons[i].name)
                    {
                        weapons[i].AddAmmo();
                        GameManager.instance.UpdateAmmoUI(weapons[selectedWeapon]);
                        return;
                    }
                }
                stats.isShooting = false;
                stats.ammoCount = 0;
                weapons.Add(stats);
                selectedWeapon = weapons.Count - 1;
                weapons[selectedWeapon].Initialize(GameManager.instance.playerScript.muzzlePoint); //this may need to change per weapon but will see
                weapons[selectedWeapon].AddAmmo();
                ChangeGun();
                return;
            }
            else
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    if (wepName == weapons[i].name)
                    {
                        return;
                    }
                }
                stats.isShooting = false;
                weapons.Add(stats);
                selectedWeapon = weapons.Count - 1;
                weapons[selectedWeapon].Initialize(GameManager.instance.playerScript.muzzlePoint);
                ChangeGun();
                return;
            }
        }
        else
        {
            stats.isShooting = false;
            weapons.Add(stats);
            selectedWeapon = weapons.Count - 1;
            weapons[selectedWeapon].Initialize(GameManager.instance.playerScript.muzzlePoint);
            if (weapons[selectedWeapon].ammoCount != -1) {
                weapons[selectedWeapon].ammoCount = 0;
                weapons[selectedWeapon].AddAmmo();
                GameManager.instance.UpdateAmmoUI(weapons[selectedWeapon]);
            }
            ChangeGun();
            return;
        }
    }
    void ShootGun() {
        if (weapons[selectedWeapon].ammoCount == -1 || weapons[selectedWeapon].magAmmoCount > 0) {
            StartCoroutine(weapons[selectedWeapon].Shoot());
        }
    }
    void ReloadGun() {
        if (weapons[selectedWeapon].Reload())
            GameManager.instance.playerScript.aud.PlayOneShot(reloadClip, .5f);
        GameManager.instance.UpdateAmmoUI(weapons[selectedWeapon]);
    }
    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weapons.Count - 1)
        {
            selectedWeapon++;
            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
        {
            selectedWeapon--;
            ChangeGun();
        }
    }
    void ChangeGun() {
        GameManager.instance.playerScript.gunModel.GetComponent<MeshFilter>().sharedMesh = weapons[selectedWeapon].model.GetComponent<MeshFilter>().sharedMesh;
        GameManager.instance.playerScript.gunModel.GetComponent<MeshRenderer>().sharedMaterial = weapons[selectedWeapon].model.GetComponent<MeshRenderer>().sharedMaterial;
        GameManager.instance.DisplayGunImage(weapons[selectedWeapon].image);
        GameManager.instance.UpdateAmmoUI(weapons[selectedWeapon]);
    }

    //public bool Upgrade(string name)
    //{
    //    bool result = false;
    //    for (int i = 0; i < items.Length; i++)
    //    {
    //        if (items[i].name == name)
    //        {
    //            if (items[i].has && items[i].upgraded)
    //                return result;
    //            else if (items[i].has)
    //            {
    //                result = true;
    //                items[i].upgraded = result;
    //            }
    //            else {
    //                result = true;
    //                items[i].has = result;
    //            }
    //            break;

    //        }
    //    }
    //    return result;
    //}

    public void AddToPersistenceManager()
    {
        PersistenceManager.instance.AddToManager(this);
    }
    public void SaveState()
    {
        PlayerPrefs.SetInt("SelectedWeapon", selectedWeapon);
        PersistenceManager.instance.SaveInventoryWeapons(weapons);
    }    
    
    public void LoadState()
    {
        weapons.Clear();
        weapons = PersistenceManager.instance.LoadInventoryWeapons();
        selectedWeapon = PlayerPrefs.GetInt("SelectedWeapon", 0);

        if(weapons.Count > selectedWeapon)
        {
            ChangeGun();
        }
    }

}
