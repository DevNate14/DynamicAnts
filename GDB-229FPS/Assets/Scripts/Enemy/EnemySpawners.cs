using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawners : MonoBehaviour
{
    [SerializeField] GameObject EnemyToSpawn;
    [SerializeField] int EnemyToSpawnCount;
    [SerializeField] int EnemySpawnDelay;
    [SerializeField] Transform[] SpawnPos;
    [SerializeField] List<GameObject> SpawnList = new List<GameObject>();
    int spawncount;
    bool IsSpawning;
    bool StartSpawn;


    void Start()
    {
        GameManager.instance.UpdateGameGoal(EnemyToSpawnCount);
    }
    private void Update()
    {
        if (StartSpawn && spawncount < EnemyToSpawnCount && !IsSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }
    IEnumerator SpawnEnemies()
    {
        IsSpawning = true;
        int spawnpoint = Random.Range(0, SpawnPos.Length - 1);
        GameObject temp = Instantiate(EnemyToSpawn, SpawnPos[spawnpoint].transform.position, SpawnPos[spawnpoint].transform.rotation);
        // I do not understand why this breaks things at all 
        EnemyAI thing1 = temp.GetComponent<EnemyAI>();
        if (thing1 != null)
        {
            thing1.mySpawner = this;
        }
        Turret thing2 = temp.GetComponent<Turret>();
        if (thing2 != null)
        {
            thing2.mySpawner = this;
        }
        MeleeEnemy thing3 = temp.GetComponent<MeleeEnemy>();
        if (thing3 != null)
        {
            thing3.mySpawner = this;
        }
        //CheckType(temp);
        SpawnList.Add(temp);
        spawncount++;
        yield return new WaitForSeconds(EnemySpawnDelay);
        IsSpawning = false;
    }
    //void CheckType(GameObject obj) {
    //    EnemyAI thing1 = obj.GetComponent<EnemyAI>();
    //    if (thing1 != null) { 
    //        thing1.mySpawner = this; 
    //    }
    //    Turret thing2 = obj.GetComponent<Turret>();
    //    if (thing2 != null) { 
    //        thing2.mySpawner = this;
    //    }
    //    MeleeEnemy thing3 = obj.GetComponent<MeleeEnemy>();
    //    if (thing3 != null) { 
    //        thing3.mySpawner = this; 
    //    }
    //}
    public void DeadUpdate()
    {
        EnemyToSpawnCount--;
        spawncount--;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartSpawn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartSpawn = false;
            foreach (GameObject obj in SpawnList) {
                Destroy(obj);
            }
            SpawnList.Clear();
            spawncount = 0;
        }
    }
}
