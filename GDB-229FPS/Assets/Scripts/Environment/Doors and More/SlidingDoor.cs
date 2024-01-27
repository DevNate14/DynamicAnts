using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlidingDoor : Door
{
    [SerializeField] Animation Anim;
    [SerializeField] bool OpenOnly;
    [SerializeField] float Cooldown;
    bool CanOpen = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                    PlayOpen();
                }
            }
            else
            {
                Open = false;
                Anim.Play("close");
                PlayClose();
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
}
