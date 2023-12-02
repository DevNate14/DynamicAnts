using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory
{
    Weapon[] weapons = new Weapon[5]; // same with this one
    int CurrItem, CurrWeapon; // this is for the inventories tracking 
    public int SelectedWeapon; // this is for player tracking, will be handled in player might just put in player

    public Weapon GetWeapon() {
        if (CurrWeapon == 0) {
            return null;
        }
        if (weapons[SelectedWeapon-1] != null) {
            return weapons[SelectedWeapon - 1];
        }
        return null;
    }
    public void SetSelectedWeapon(int num) {
        SelectedWeapon = num;
    }
    public void PickUpWeapon(Weapon weapon) {
        if (weapon.GetAmmo() != -1) {
            for (int i = 0; i < weapons.Length; i++) {
                if (weapon.GetName() == weapons[i].GetName()) {
                    weapons[i].AddAmmo();
                    return;
                }
            }
        }
        weapons[CurrWeapon] = weapon;
        if (weapon.GetAmmo() != -1) { weapons[CurrWeapon].AddAmmo(); }
        CurrWeapon++;
    }
}
