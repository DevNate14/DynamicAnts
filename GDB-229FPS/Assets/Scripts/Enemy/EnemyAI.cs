using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] int enemyJump;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    Vector3 playerDirection;
    bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            agent.SetDestination(GameManager.instance.Player.transform.position);
        }
    }

    public void Damage(int amount)
    {
        HP -= amount;
        DamageFeedback();
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Heal(int amount)
    {
        HP += amount;
        if (HP >= 10)
        {
            HP = 10;
        }
    }
    IEnumerator DamageFeedback()
    {
        Color Temp = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Temp;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
