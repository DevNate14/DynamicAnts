using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class Pit : MonoBehaviour
{
    [SerializeField] float WaitTime; // time after player falls into pit before the player loses all health
    bool Triggered;
    Vector3 CamRelativePos;
    Vector3 LockedCamPos;
    GameObject Camera;

    private void Start()
    {
        Camera = GameObject.FindWithTag("MainCamera");
    }

    private void Update()
    {
        if (Triggered)
        {
            Camera.transform.position = LockedCamPos;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Triggered)
        {
            IDamageable player = other.GetComponent<IDamageable>();
            if (player != null)
            {
                if (Camera != null) //just in case
                {
                    LockedCamPos = Camera.transform.position;
                    CamRelativePos = Camera.transform.localPosition;
                    Triggered = true;
                }
                StartCoroutine(Kill(player));
            }
        }
    }

    IEnumerator Kill(IDamageable player)
    {
        yield return new WaitForSeconds(WaitTime);
        player.Damage(999);
        Triggered = false;
        Camera.transform.localPosition = CamRelativePos;
    }
}
