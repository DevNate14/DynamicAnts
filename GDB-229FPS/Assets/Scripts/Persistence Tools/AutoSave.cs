using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{ 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PersistenceManager.instance.SaveGame();
        }
    }
}
