using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] bool TeleportType;
    public bool IsTeleporting;
    [SerializeField] float TeleportWaitTime;

    public bool Locked;
    public bool Open;


    public virtual void Interact()
    {
        
    }

    public virtual void Teleport()
    {
        
    }
}
