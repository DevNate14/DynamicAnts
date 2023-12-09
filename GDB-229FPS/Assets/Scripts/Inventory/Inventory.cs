using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory
{
    List<Weapon> weapons = new List<Weapon>();
    int selectedWeapon;
    private void Update() {
        SelectGun();
    }
    public void PickUpWeapon(Weapon weapon) {
        string wepName = weapon.GetName();
        if (weapons.Count != 0)
        {
            if (weapon.GetAmmo() != -1)
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    if (wepName == weapons[i].GetName())
                    {
                        weapons[i].AddAmmo();
                        return;
                    }
                }
                selectedWeapon = weapons.Count;
                weapons.Add(weapon);
                ChangeGun(true);
                return;
            }
            else {
                for (int i = 0; i < weapons.Count; i++)
                {
                    if (wepName == weapons[i].GetName())
                    {
                        return;
                    }
                }
                selectedWeapon = weapons.Count;
                weapons.Add(weapon);
                ChangeGun(true);
                return;
            }
        }
        else {
            weapons.Add(weapon);
            ChangeGun(true);
            return;
        }
    }
    void SelectGun() {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weapons.Count-1) {
            selectedWeapon++;
            ChangeGun(false);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon > 0) {
            selectedWeapon--;
            ChangeGun(false);
        }
    }
    void ChangeGun(bool isFirstPickup) {
        if (isFirstPickup) {
            Instantiate(weapons[selectedWeapon].stats.model, GameManager.instance.playerScript.GunAttachPoint.transform);
        }
    }
}
