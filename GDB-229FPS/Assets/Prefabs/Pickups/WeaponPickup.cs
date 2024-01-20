using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IPersist
{
    [SerializeField] GunStatsSO gun;
    bool triggerSet; // to prevent the same bug from lecture
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerSet) {
            triggerSet = true;
            var comp = other.GetComponent<Inventory>();
            comp.PickUpWeapon(gun);
            PlayerPrefs.SetInt(this.gameObject.GetInstanceID().ToString() + "PickedUp", 0);
            Destroy(gameObject);
        }
    }
    public void AddToPersistenceManager()
    {
        PersistenceManager.instance.AddToManager(this);
    }
    public void SaveState()
    {
        PlayerPrefs.SetInt(this.gameObject.GetInstanceID().ToString() + "PickedUp", 0);
    }

    public void LoadState()
    {
        if(PlayerPrefs.GetInt(this.gameObject.GetInstanceID().ToString() + "PickedUp", 0) == 1)
        {
            Destroy(gameObject);
        }
    }
}