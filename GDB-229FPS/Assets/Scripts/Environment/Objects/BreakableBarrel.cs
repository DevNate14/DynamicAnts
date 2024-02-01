using System.Collections.Generic;
using UnityEngine;

public class BreakableBarrel : BreakableObject
{
    [SerializeField] GameObject Barrel;
    [SerializeField] Collider Hitbox;
    [SerializeField] GameObject BrokenBarrel;
    [SerializeField] List<GameObject> DroppableObjects = new List<GameObject>();
    [SerializeField] int DropRate;
    public override void Break()
    {
        BrokenBarrel.SetActive(true);
        Barrel.SetActive(false);
        Hitbox.enabled = false;
        Instantiate(BreakFX, transform.position, transform.rotation);
        Aud.PlayOneShot(Sound);
        if (DroppableObjects.Count > 0 && Random.Range(0, 100) <= DropRate)
            Instantiate(DroppableObjects[Random.Range(0, DroppableObjects.Count - 1)], transform.position, transform.rotation);
        Destroy(BrokenBarrel, 5);
    }
}
