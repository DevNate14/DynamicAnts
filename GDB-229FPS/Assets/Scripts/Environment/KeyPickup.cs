using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] int keyId;
    void Start()
    {
        LoadState();
    }
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<Inventory>().PickUpKey(keyId);
            Destroy(gameObject);
        }
    }
    public void LoadState()
    {
        if(PlayerPrefs.GetString("Keys", "000")[keyId - 1] == '1')
        {
            Destroy(gameObject);
        }    
    }
}
