using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory 
{
    void PickUpItem(Item Item);
    void PickUpWeapon(Weapon Weapon);
}
