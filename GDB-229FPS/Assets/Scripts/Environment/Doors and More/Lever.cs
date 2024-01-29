using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] List<Door> Doors;
    [SerializeField] Collider Hitbox;
    bool SingleUse;
    [SerializeField] Animation Anim;
    bool On;
    bool Animating;
    [SerializeField] float Cooldown;
    [SerializeField] bool Unlocking;
    [SerializeField] bool Opening;

    public void Interact()
    {
        if (Opening)
        {
            foreach (Door door in Doors)
            {
                if (Unlocking)
                    door.Locked = !door.Locked;
                door.Interact();
            }
        }
        if (!Animating)
        {
            StartCoroutine(Animate());
        }
        if (SingleUse)
        {
            Hitbox.enabled = false;
        }
    }
    IEnumerator Animate()
    {
        Animating = true;
        if (!On)
            Anim.Play("forward");
        else
            Anim.Play("backward");
        yield return new WaitForSeconds(Cooldown);
        On = !On;
        Animating = false;
    }
}
