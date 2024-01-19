using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour, IPersist
{
    [SerializeField] MeshRenderer cylinder;
    [SerializeField] Material inactiveMaterial;
    [SerializeField] Material activeMaterial;
    
    public bool active;


    private void Start()
    {
        AddToPersistenceManager();
        LoadState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject[] otherCheckPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPos");
            foreach (GameObject checkPoint in otherCheckPoints)
            {
                checkPoint.GetComponent<CheckPoint>().Deactivate();
            }
            active = true;
            cylinder.material = activeMaterial;
            GameManager.instance.playerScript.playerSpawnPos = transform.position;
            PersistenceManager.instance.SaveGame();
        }
    }

    public void Deactivate()
    {
        active = false;
        cylinder.material = inactiveMaterial;
    }

    public void AddToPersistenceManager()
    {
        PersistenceManager.instance.AddToManager(this);
    }
    public void SaveState()
    {
        if (active) { PlayerPrefs.SetInt("ActiveCheckPoint", this.gameObject.GetInstanceID()); }
    }

    public void LoadState()
    {
        active = PlayerPrefs.GetInt("ActiveCheckPoint") == this.gameObject.GetInstanceID();
        if(active)
        {
            cylinder.material = activeMaterial;
        }
        else
        {
            cylinder.material = inactiveMaterial;
        }
    }
}
