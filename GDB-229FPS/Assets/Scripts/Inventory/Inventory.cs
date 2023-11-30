using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory
{
    Item[] items = new Item[5]; // we can change this if we end up with more items
    Weapon[] weapons = new Weapon[5]; // same with this one
    int CurrItem, CurrWeapon; // this is for the inventories tracking 
    public int SelectedWeapon; // this is for player tracking, will be handled in player might just put in player

    public void PickUpItem(Item item) { // this should cover adding all items, if we limit placement and only give needed plus like 10-20% this should not run into problems
                                        // only thing this doesnt cover is items that would need a second stack/spot in the inventory. Items will
        for (int i = 0; i < items.Length; i++) {
            if (item.GetName() == items[i].GetName()) {
                items[i].AddUse();
                return;
            }
        }
        items[CurrItem] = item;
        CurrItem++;
    }
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
    public int FindItem(Item item) {
        int ndx = 0;
        foreach (var v in items) {
            if (v.GetName() == item.GetName()) {
                return ndx;
            }
            ndx++;
        }
        return -1;
    }
    public void UpgradeItem(int ndx) {
        IUpgradable upg = items[ndx].GetComponent<IUpgradable>();
        if (upg != null) {
            upg.Upgrade();
        }
    }
}
