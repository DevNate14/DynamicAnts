using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProximityOpen : MonoBehaviour
{
    [SerializeField] Door Door;
    [SerializeField] MonoBehaviour IdentifyingScript;

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;
        if (other.GetComponent(IdentifyingScript.GetType()) != null)
        {
            Debug.Log("Door is Open");
            Door.Open = true;
        }
        else
        {
            Debug.Log("Nuh uh");
            Door.Open = false;
        }
    }
}
