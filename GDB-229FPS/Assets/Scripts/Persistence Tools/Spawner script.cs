using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnerscript : MonoBehaviour
{
    //tbc events/event system needed, persistence partial rework required. need to finish making implentation of this class for objects that have permanent states once level is left, this can bleed over into enemies to make that easier 
    [SerializeField] public int id;
    [SerializeField] GameObject heldObj;
    [SerializeField] public bool shouldSpawn;

    public void OnLoad(bool setSpawn){
        shouldSpawn = setSpawn;
        heldObj.SetActive(shouldSpawn);
    }
}
