using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProximityOpen : MonoBehaviour
{
    [SerializeField] Door Door;
    [SerializeField] bool OpenOnly;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (OpenOnly && !Door.Open)
            {
                Door.Interact();
            }
            else if (!Door.Open)
            {
                Door.Interact();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (OpenOnly && Door.Open)
            {
                Door.Interact();
            }
            else if (Door.Open)
            {
                Door.Interact();
            }
        }
    }

}
