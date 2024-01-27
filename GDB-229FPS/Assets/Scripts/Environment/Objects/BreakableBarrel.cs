using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBarrel : BreakableObject
{
    [SerializeField] GameObject Barrel;
    [SerializeField] Collider Hitbox;
    [SerializeField] GameObject BrokenBarrel;


    public override void Break()
    {
        BrokenBarrel.SetActive(true);
        Barrel.SetActive(false);
        Hitbox.enabled = false;
        Instantiate(BreakFX, transform.position, transform.rotation);
        Aud.PlayOneShot(Sound);
    }
}
