using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField] protected GameObject storedItem;
    [SerializeField] bool playerOnly, isUpgrade;

    private void Awake() {
       //StoredItem = Instantiate<GameObject>(StoredItem, Pos.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) {
            return;
        }
        //if (other.CompareTag("Player")) { // currently only implemented players portion for weapons, will write up in chat what is missing
        //    //if (StoredItem.CompareTag("Item")) {
        //    //    if (IsUpgrade) {
        //    //       // mv.UpgradeItem();
        //    //    }
        //    //}
        //    if (StoredItem.CompareTag("Weapon")) {
        //        GameManager.instance.PlayerScript.Inventory.PickUpWeapon();
        //        Destroy(gameObject);
        //    }
        //}
        Activate();
        Destroy(gameObject);
    }
    public abstract void Activate();
}
