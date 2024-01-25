using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class RotatingDoor : Door
{
    [SerializeField] Animation Anim;
    [SerializeField] bool OpenRight;
    [SerializeField] float Cooldown;
    bool CanOpen = true;
    public override void Interact()
    {
        if (CanOpen)
        {
            if (!Open)
            {
                if (!Locked)
                {
                    Open = true;
                    Anim.Play("open");
                }
            }
            else
            {
                Open = false;
                Anim.Play("close");
            }
            StartCoroutine(OpenCooldown());
        }
    }

    IEnumerator OpenCooldown()
    {
        CanOpen = false;
        yield return new WaitForSeconds(Cooldown);
        CanOpen = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Interact();
    //}
    // for testing purposes
}
