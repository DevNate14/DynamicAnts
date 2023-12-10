using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    // put this on an item script if you want there to be upgrades for it, then make a pick up to call it if the player has it, could get kind of complex with this if you want to.
    // have to implement this on your weapon or object to be used/upgraded will have example with the long jump item.
    bool Upgrade(string name);
    void PickUpItem(upgradeItem item);
}
