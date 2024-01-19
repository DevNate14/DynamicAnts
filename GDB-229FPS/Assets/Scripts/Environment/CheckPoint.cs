using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour, IPersist
{
    [SerializeField] MeshRenderer colliderEffect;
    [SerializeField] Material colliderInactive;
    [SerializeField] Material colliderActive;

    [SerializeField] MeshRenderer baseEffect;
    [SerializeField] Material baseInactive;
    [SerializeField] Material baseActive;

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
            colliderEffect.material = colliderActive;
            baseEffect.material = baseActive;

            GameManager.instance.playerScript.playerSpawnPos = transform.position;
            PersistenceManager.instance.SaveGame();
        }
    }

    public void Deactivate()
    {
        active = false;
        colliderEffect.material = colliderInactive;
        baseEffect.material = baseInactive;
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
            colliderEffect.material = colliderActive;
            baseEffect.material = baseActive;
        }
        else
        {
            colliderEffect.material = colliderInactive;
            baseEffect.material = baseInactive;
        }
    }
}
