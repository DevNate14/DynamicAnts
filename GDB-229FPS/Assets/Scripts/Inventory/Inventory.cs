using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory, IUpgradable
{
    [SerializeField] List<GunStatsSO> weapons = new List<GunStatsSO>();    
    [SerializeField] UpgradeItem[] items;
    int selectedWeapon;
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
                        return;
                    }
                }
                selectedWeapon = weapons.Count-1;
                stats.isShooting = false;
                weapons.Add(stats);
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
                selectedWeapon = weapons.Count-1;
                stats.isShooting = false;
                weapons.Add(stats);
                weapons[selectedWeapon].Initialize(GameManager.instance.playerScript.muzzlePoint);
                ChangeGun();
                return;
            }
        }
        else
        {
            stats.isShooting = false;
            weapons.Add(stats);
            weapons[selectedWeapon].Initialize(GameManager.instance.playerScript.muzzlePoint);
            if(weapons[selectedWeapon].ammoCount != -1)
                weapons[selectedWeapon].AddAmmo();
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
        weapons[selectedWeapon].Reload();
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
    }

    public bool Upgrade(string name)
    {
        bool result = false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].name == name)
            {
                if (items[i].has && items[i].upgraded)
                    return result;
                else if (items[i].has)
                {
                    result = true;
                    items[i].upgraded = result;
                }
                else {
                    result = true;
                    items[i].has = result;
                }
                break;
                
            }
        }
        return result;
    }
}
