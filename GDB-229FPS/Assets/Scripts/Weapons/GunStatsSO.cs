using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.UI;
//using Microsoft.Unity.VisualStudio.Editor;

[CreateAssetMenu]
public abstract class GunStatsSO : ScriptableObject
{
    [Header("----- Stats -----")]
    public new string name;
    public int magSize, ammoCount, magAmmoCount;
    public float fireRate;

    //range is handled in the bullets per weapon

    [Header("----- Assets -----")]
    public Texture2D image; // Image for UI
    public GameObject bullet, model;
    public ParticleSystem muzzleEffect;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootVol;

    public GameObject muzzlePoint;// this may need to be replaced with a parameter in the shoot function

    public bool isShooting;

    public void Initialize(GameObject point) {
        muzzlePoint = point;
    }

    //---------------------------------------------------- CODE FOR UI FUNCTIONS ---------------------------------------------------------------------------------

    public void DisplayGunImage(RawImage rawImage)
    {
        //References to Textures that are added in the GunSO on Unity
        {
            if (image != null && rawImage != null)
            {
                rawImage.texture = image;
            }
        }
    } //Need to add code for Weapon Pick-up
    
    public virtual void Reload()
    {// virtual so if you need the ability to change this for your weapon you can
        if (ammoCount == 0)
        {
            GameManager.instance.ReloadUI();
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

     //---------------------------------------------------- CODE FOR UI FUNCTIONS ---------------------------------------------------------------------------------

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
