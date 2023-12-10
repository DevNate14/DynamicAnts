using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct upgradeItem
{
    public string name;
    public bool has, upgraded;
}

public class Inventory : MonoBehaviour, IInventory, IUpgradable
{
    List<GunStatsSO> weapons = new List<GunStatsSO>();
    List<upgradeItem> items = new List<upgradeItem>(); 
    int selectedWeapon;
    private void Update() {
       SelectGun();
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
                stats.AddAmmo();
                weapons.Add(stats);
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
                weapons.Add(stats);
                ChangeGun();
                return;
            }
        }
        else
        {
            weapons.Add(stats);
            ChangeGun();
            return;
        }
    }
    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weapons.Count - 1 && !weapons[selectedWeapon].isShooting)
        {
            selectedWeapon++;
            ChangeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon > 0 && !weapons[selectedWeapon].isShooting)
        {
            selectedWeapon--;
            ChangeGun();
        }
    }
    void ChangeGun()
    {
        GameManager.instance.playerScript.gunModel.GetComponent<MeshFilter>().sharedMesh = weapons[selectedWeapon].model.GetComponent<MeshFilter>().sharedMesh;
        GameManager.instance.playerScript.gunModel.GetComponent<MeshRenderer>().sharedMaterial = weapons[selectedWeapon].model.GetComponent<MeshRenderer>().sharedMaterial;

    }

    public bool Upgrade(string name)
    {
        bool result = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == name)
            {
                if (items[i].has && items[i].upgraded)
                    return result;
                else if (items[i].has) {
                    result = true;
                    //items[i].upgraded = result; giving error saying Im trying to acces a reference type, will need to come back and fix this when I get the chance after guns are finished;
                }
                break;
                
            }
        }
        return result;
    }

    public void PickUpItem(upgradeItem item)
    {
    }
}
