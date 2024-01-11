using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class MeleeWeapon : GunStatsSO
{
    public override bool Reload() { return false; }
    public override IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(bullet, muzzlePoint.transform.position, muzzlePoint.transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
        if (ammoCount != -1)
        { // checking for infinite ammo weapons
            ammoCount--;
        }
    }
}