using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInventory
{
    List<GunStatsSO> weapons = new List<GunStatsSO>();
    [SerializeField] GameObject gunModel;
    int selectedWeapon;
    private void Update() {
       // SelectGun();
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
                selectedWeapon = weapons.Count;
                weapons.Add(stats);
                //  ChangeGun();
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
                selectedWeapon = weapons.Count;
                weapons.Add(stats);
                // ChangeGun();
                return;
            }
        }
        else
        {
            weapons.Add(stats);
            //ChangeGun();
            return;
        }
    }
    //void SelectGun() {
    //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weapons.Count-1) {
    //        selectedWeapon++;
    //        ChangeGun();
    //    }
    //    else if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon > 0) {
    //        selectedWeapon--;
    //        ChangeGun();
    //    }
    //}
    void ChangeGun()
    {

    }
}
