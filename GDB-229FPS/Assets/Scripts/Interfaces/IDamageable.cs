using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // put this on an enemy, prop, or anything you want to be able to damage or kill. call to heal or damage said object. *removed heal since damage negative does same*
    void Damage(int amount);
}
