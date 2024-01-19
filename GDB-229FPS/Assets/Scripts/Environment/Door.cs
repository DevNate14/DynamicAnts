using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public bool Locked;
    public bool Open;


    public virtual void Interact()
    {
        
    }
}
