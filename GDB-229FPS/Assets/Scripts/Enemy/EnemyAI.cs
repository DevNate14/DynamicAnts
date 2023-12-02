using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] int enemyJump;
    [Range(1, 5)][SerializeField] float timebetweenattacks;
    [Range(1, 4)][SerializeField] int damageRange;
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
            StartCoroutine(DamageOnProximity());
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
    public IEnumerator DamageOnProximity()
    {
        //calculates how far the player and enemy are from each other 
            float distanceToPlayer = Vector3.Distance(transform.position, GameManager.instance.Player.transform.position);
        // if the distance is less than the damage range do damage then wait 5 sec I made time & range serialized fields so we can adjust depending on how we feel about it
            if (distanceToPlayer < damageRange)
            {
                GameManager.instance.PlayerScript.Damage(1);                
                yield return new WaitForSeconds(timebetweenattacks);
            }
    }
}
