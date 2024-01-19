using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class RotatingDoor : Door
{
    bool OpenRight;
    public override void Interact()
    {
        if (!Locked)
        {
            if (!Open)
            {
                Open = true;
                transform.rotation = Quaternion.LookRotation(-transform.right);
            }
            else
            {
                Open = true;
                transform.rotation = Quaternion.LookRotation(transform.right);
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Interact();
    //}
    // for testing purposes
}
