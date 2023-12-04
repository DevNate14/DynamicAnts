using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable //check
{

    [Range(1, 10)][SerializeField] int HP; //check
    [Range(1, 10)][SerializeField] float shootrate; //check
    [SerializeField] Transform ShootPos; //check
    [SerializeField] Renderer model; //check
    [SerializeField] NavMeshAgent agent; // check
    [SerializeField] GameObject bullet; // check
    //[SerializeField] Transform headPosition;
    [SerializeField] int viewCone;//check
    Vector3 playerDirection; //check
    bool playerInRange; // check
    bool shooting; //check
    float angleToPlayer; // check
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame4
    void Update()
    {
        if(playerInRange && canSeePlayer()) //check
        {

        }   
    }

    bool canSeePlayer() //check
    {
        playerDirection = GameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.DrawRay(transform.position, playerDirection);
        Debug.Log(angleToPlayer);
        RaycastHit hit;

        if(Physics.Raycast(transform.position, playerDirection, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                
                agent.SetDestination(GameManager.instance.player.transform.position); //check

                if(!shooting)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        return false;
    }


    public void Damage(int amount) //check
    {
        HP -= amount;
        StartCoroutine(DamageFeedback());
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Heal(int amount) //check
    {
        HP += amount;
        if (HP >= 10)
        {
            HP = 10;
        }
    }
    public void OnTriggerEnter(Collider other) //check
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    public void OnTriggerExit(Collider other) //check
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    
    IEnumerator shoot()
    {
       shooting = true;
       Instantiate(bullet, ShootPos.position, transform.rotation);
       yield return new WaitForSeconds(shootrate);

       shooting = false;
    }
    
    IEnumerator DamageFeedback() //Check
    {
        Color Temp = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Temp;
    }
}
