using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowBar : Weapon
{
    public override void Reload()
    {
        //base.Reload();
        // doing this so nothing happens on reload since it is a melee weapon
    }
    public override void Shoot()
    {
        base.Shoot();
    }
}
