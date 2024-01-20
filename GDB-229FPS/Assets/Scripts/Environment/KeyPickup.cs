using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] int keyId;
    void Start()
    {
        
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Inventory>().PickUpKey(keyId);
            Destroy(gameObject);
        }
    }
}
