using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IPersist
{
    [SerializeField] GunStatsSO gun;
    [SerializeField] GameObject SoundObject;
    bool triggerSet; // to prevent the same bug from lecture

    private void Start()
    {
        AddToPersistenceManager();
        LoadState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerSet) {
            triggerSet = true;
            var comp = other.GetComponent<Inventory>();
            comp.PickUpWeapon(gun);
            PlayerPrefs.SetInt(this.gameObject.GetInstanceID().ToString() + "PickedUp", 1);
            if (SoundObject != null)
                Instantiate(SoundObject, transform.position, transform.rotation);
            gameObject.SetActive(false);
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
