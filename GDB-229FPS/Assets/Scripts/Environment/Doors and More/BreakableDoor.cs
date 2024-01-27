using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BreakableDoor : BreakableObject
{
    [SerializeField] GameObject Door;
    [SerializeField] Collider Hitbox;
    [SerializeField] GameObject BrokenDoor;
    public override void Break()
    {
        BrokenDoor.SetActive(true);
        Door.SetActive(false);
        Hitbox.enabled = false;
        Instantiate(BreakFX, transform.position, transform.rotation);
        Aud.PlayOneShot(Sound);
    }
}
