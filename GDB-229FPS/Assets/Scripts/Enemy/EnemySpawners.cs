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
        SpawnList.Add(temp);
        spawncount++;
        yield return new WaitForSeconds(EnemySpawnDelay);
        IsSpawning = false;
    }
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
            foreach (GameObject obj in SpawnList) {Destroy(obj);}
        }
    }
}
