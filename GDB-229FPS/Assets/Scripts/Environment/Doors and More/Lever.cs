using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] List<Door> Doors;
    [SerializeField] Collider Hitbox;
    bool SingleUse;
    [SerializeField] bool Unlocking;
    [SerializeField] bool Opening;

    public void Interact()
    {
        if (Unlocking && Opening)
        {
            foreach (Door door in Doors)
            {
                door.Locked = !door.Locked;
                door.Interact();
            }
        }
        else if (Unlocking)
        {
            foreach (Door door in Doors)
                door.Locked = !door.Locked;
        }
        else if (Opening)
        {
            foreach (Door door in Doors)
                door.Interact();
        }

        if (SingleUse)
        {
            Hitbox.enabled = false;
        }
    }
}
