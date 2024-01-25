using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class RotatingDoor : Door
{
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
                    transform.rotation = Quaternion.LookRotation(-transform.right);
                }
            }
            else
            {
                Open = false;
                transform.rotation = Quaternion.LookRotation(transform.right);
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
