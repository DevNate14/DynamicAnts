using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Treadmill : MonoBehaviour
{
    [SerializeField] float Speed;
    private void OnTriggerStay(Collider other)
    {
        other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position + transform.forward * transform.localScale.x * 1.2f, Speed * Time.deltaTime);
        // if this isnt good enough, i could try to find a way to get it to only move forward instead of toward the center of the front of the trigger
    }
}
