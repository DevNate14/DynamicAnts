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
    [SerializeField] int Damage, MagSize, AmmoCount, MagAmmoCount;
    //[SerializeField] bool GivesImpulseEnemy, GivesImpulsePlayer;  was going to store a vector 3 for each of these but that is wasteful, if these are true for your weapon I would add a hard coded version of this since I am unsure how many weapons will actually use this
    [SerializeField] float FireRate;
    [SerializeField] Range GunRange;
    [SerializeField] bool HasRecoil; // if wanted on your weapon you can have recoil reduce the players velocity some. might remove this if no weapons use
    [SerializeField] Vector3 RecoilImpulse;
    [SerializeField] GameObject MuzzlePoint;
    [SerializeField] string Name;
    [SerializeField] GameObject testObject; // object to spawn to just test out the raycast

    public bool isShooting;

    public virtual void Reload() {// virtual so if you need the ability to change this for your weapon you can
        if (AmmoCount == 0) {
            // error or blinking UI telling player that ammo is empty

            return;
        }
        if (AmmoCount < (MagSize - MagAmmoCount)) {
            // if we wanted an animation itd be added above this if with an IEnum that does movement then reloads
            MagAmmoCount += AmmoCount;
            AmmoCount = 0;
            // in individual classes we can add a small movement or UI flash saying ammo was refilled and showing player ammo is being reloaded
            // can make this more complex later.
        }
        else{
            AmmoCount -= (MagSize - MagAmmoCount);
            MagAmmoCount = MagSize;
        }
    }

    // VV this may need to be filled out per weapon as it will could be different, idea is you make the IEnum per gun and then cast shoot per bullet needed
    public virtual void Shoot() {
        isShooting = true; // <
        RaycastHit hit;
        if (Physics.Raycast(MuzzlePoint.transform.position, MuzzlePoint.transform.forward, out hit, (float)GunRange)) {
            IDamageable trgt = hit.collider.GetComponent<IDamageable>();
            if (trgt != null) { // this is the damage per bullet, shotgun will either repeat this the number of pellets it has or do it in one monolithic block, need to see plus range damage drop off, check that script for example
                trgt.Damage(Damage);
                
            }
            Instantiate<GameObject>(testObject, hit.transform);
        }
        isShooting = false; // < these may need to be removed and added to individuals but testing for now
        if (AmmoCount != -1) { // checking for infinite ammo weapons
            AmmoCount--;
        }
    }
    public void AddAmmo() {
        AmmoCount += MagSize * 3;
    }
    public int GetAmmo() {
        return AmmoCount;
    }
    // need to make an IEnumerator per weapon because each will be different
    public string GetName() {
        return Name;
    }
}
