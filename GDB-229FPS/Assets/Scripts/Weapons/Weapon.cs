using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public GunStatsSO stats;
    [SerializeField] GameObject MuzzlePoint;

    public bool isShooting;

    public virtual void Reload() {// virtual so if you need the ability to change this for your weapon you can
        if (stats.ammoCount == 0) {
            // error or blinking UI telling player that ammo is empty

            return;
        }
        if (stats.ammoCount < (stats.magSize - stats.magAmmoCount)) {
            // if we wanted an animation itd be added above this if with an IEnum that does movement then reloads
            stats.magAmmoCount += stats.ammoCount;
            stats.ammoCount = 0;
            // in individual classes we can add a small movement or UI flash saying ammo was refilled and showing player ammo is being reloaded
            // can make this more complex later.
        }
        else{
            stats.ammoCount -= (stats.magSize - stats.magAmmoCount);
            stats.magAmmoCount = stats.magSize;
        }
    }

    // VV this may need to be filled out per weapon as it will could be different, idea is you make the IEnum per gun and then cast shoot per bullet needed
    public virtual void Shoot() {
        isShooting = true; // <
        RaycastHit hit;
        if (Physics.Raycast(MuzzlePoint.transform.position, MuzzlePoint.transform.forward, out hit, (float)stats.gunRange)) {
            IDamageable trgt = hit.collider.GetComponent<IDamageable>();
            if (trgt != null) { // this is the damage per bullet, shotgun will either repeat this the number of pellets it has or do it in one monolithic block, need to see plus range damage drop off, check that script for example
                trgt.Damage(stats.damage);
                
            }
        }
        isShooting = false; // < these may need to be removed and added to individuals but testing for now
        if (stats.ammoCount != -1) { // checking for infinite ammo weapons
            stats.ammoCount--;
        }
    }
    public void AddAmmo() {
        stats.ammoCount += stats.magSize * 3;
    }
    public int GetAmmo() {
        return stats.ammoCount;
    }

    public string GetName() {
        return stats.name;
    }
}
