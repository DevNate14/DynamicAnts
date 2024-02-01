using System.Collections;
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
        yield return new WaitForSecondsRealtime(Cooldown);
        CanOpen = true;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    Interact();
    //}
    // for testing purposes
}
