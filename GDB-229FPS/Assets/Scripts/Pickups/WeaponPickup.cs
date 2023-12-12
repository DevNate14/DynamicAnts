using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] GunStatsSO gun;
    bool triggerSet; // to prevent the same bug from lecture
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerSet) {
            triggerSet = true;
            var comp = other.GetComponent<Inventory>();
            comp.PickUpWeapon(gun);
            Destroy(gameObject);
        }
    }

    
}
