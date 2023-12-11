using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GunStatsSO : ScriptableObject
{
    [Header("----- Stats -----")]
    public new string name;
    public int magSize, ammoCount, magAmmoCount;
    public float fireRate;
    //range is handled in the bullets per weapon

    [Header("----- assets -----")]
    public Texture image; // image for UI
    public GameObject bullet, model;
    public ParticleSystem muzzleEffect;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootVol;

    public GameObject muzzlePoint;// this may need to be replaced with a parameter in the shoot function

    public bool isShooting;

    public void Initialize(GameObject point) {
        muzzlePoint = point;
    }

    public virtual void Reload()
    {// virtual so if you need the ability to change this for your weapon you can
        if (ammoCount == 0)
        {
            // error or blinking UI telling player that ammo is empty

            return;
        }
        if (ammoCount < (magSize - magAmmoCount))
        {
            // if we wanted an animation itd be added above this if with an IEnum that does movement then reloads
            magAmmoCount += ammoCount;
            ammoCount = 0;
            // in individual classes we can add a small movement or UI flash saying ammo was refilled and showing player ammo is being reloaded
            // can make this more complex later.
        }
        else
        {
            ammoCount -= (magSize - magAmmoCount);
            magAmmoCount = magSize;
        }
    }

    // VV this may need to be filled out per weapon as it will could be different, idea is you make the IEnum per gun and then cast shoot per bullet needed
    public virtual IEnumerator Shoot()
    {
        isShooting = true;
        //shoot
        Instantiate(bullet,muzzlePoint.transform.position, muzzlePoint.transform.rotation);
        //Instantiate(muzzleEffect, muzzlePoint.transform.position, muzzlePoint.transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
        if (ammoCount != -1)
        { // checking for infinite ammo weapons
            magAmmoCount--;
        }
    }
    public void AddAmmo()
    {
        ammoCount += magSize * 3;
    }
}
