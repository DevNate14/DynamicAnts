using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePickup : MonoBehaviour
{
    [SerializeField] UpgradeItem item;
    bool triggerSet;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerSet)
        {
            triggerSet = true;
            var comp = other.GetComponent<Inventory>();
            comp.Upgrade(item.name);
            Destroy(gameObject);
        }
    }
}
