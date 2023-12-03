using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ranges here keep things consistent we can add and change these as time goes on if we want other kinds of weapons
enum Range
{
    Melee = 5,
    Short = 10,
    Medium = 20,
    Long = 30
}

public abstract class Weapon : MonoBehaviour
{
    // ints storing damage being dealt per projectile, ammo count, magazine size of the gun, and amount of ammo in the current magazine
    [SerializeField] int damage, magSize, ammoCount, magAmmoCount;
    //[SerializeField] bool GivesImpulseEnemy, GivesImpulsePlayer;  was going to store a vector 3 for each of these but that is wasteful, if these are true for your weapon I would add a hard coded version of this since I am unsure how many weapons will actually use this
    [SerializeField] float fireRate;
    [SerializeField] Range gunRange;
    [SerializeField] bool hasRecoil; // if wanted on your weapon you can have recoil reduce the players velocity some. might remove this if no weapons use
    [SerializeField] Vector3 recoilImpulse;
    [SerializeField] GameObject muzzlePoint;
    [SerializeField] new string name;
    [SerializeField] GameObject testObject; // object to spawn to just test out the raycast

    public bool isShooting;

    public virtual void Reload() {// virtual so if you need the ability to change this for your weapon you can
        if (ammoCount == 0) {
            // error or blinking UI telling player that ammo is empty

            return;
        }
        if (ammoCount < (magSize - magAmmoCount)) {
            // if we wanted an animation itd be added above this if with an IEnum that does movement then reloads
            magAmmoCount += ammoCount;
            ammoCount = 0;
            // in individual classes we can add a small movement or UI flash saying ammo was refilled and showing player ammo is being reloaded
            // can make this more complex later.
        }
        else{
            ammoCount -= (magSize - magAmmoCount);
            magAmmoCount = magSize;
        }
    }

    // VV this may need to be filled out per weapon as it will could be different, idea is you make the IEnum per gun and then cast shoot per bullet needed
    public virtual void Shoot() {
        isShooting = true; // <
        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.transform.position, muzzlePoint.transform.forward, out hit, (float)gunRange)) {
            IDamageable trgt = hit.collider.GetComponent<IDamageable>();
            if (trgt != null) { // this is the damage per bullet, shotgun will either repeat this the number of pellets it has or do it in one monolithic block, need to see plus range damage drop off, check that script for example
                trgt.Damage(damage);
                
            }
            Instantiate<GameObject>(testObject, hit.transform);
        }
        isShooting = false; // < these may need to be removed and added to individuals but testing for now
        if (ammoCount != -1) { // checking for infinite ammo weapons
            ammoCount--;
        }
    }
    public void AddAmmo() {
        ammoCount += magSize * 3;
    }
    public int GetAmmo() {
        return ammoCount;
    }
    // need to make an IEnumerator per weapon because each will be different
    public string GetName() {
        return name;
    }
}
