using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class MeleeWeapon : GunStatsSO
{   
    public override void Reload() {}
    public override IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(bullet, muzzlePoint.transform.position, GameManager.instance.player.transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
        if (ammoCount != -1)
        { // checking for infinite ammo weapons
            ammoCount--;
        }
    }
}