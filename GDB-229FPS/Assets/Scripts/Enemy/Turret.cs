using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Turret : MonoBehaviour, IDamageable
{
    
    [Range(1, 10)][SerializeField] int HP; //check
    [Range(1, 10)][SerializeField] float shootrate; //check
    [SerializeField] Transform ShootPos; //check
    [SerializeField] Transform ShootPos2; //check
    [SerializeField] Renderer model; //check
    [SerializeField] NavMeshAgent agent; // check
    [SerializeField] int spintarget; //check
    [SerializeField] GameObject bullet; // check
    [SerializeField] Transform headPosition; // check
    [SerializeField] int viewCone;//check
    Vector3 playerDirection; //check
    bool playerInRange; // check
    bool shooting; //check
    float angleToPlayer; // check
    public EnemySpawners mySpawner;
    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.UpdateGameGoal(1);
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
        playerDirection = GameManager.instance.player.transform.position - headPosition.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.DrawRay(headPosition.position, playerDirection);
        //Debug.Log(angleToPlayer);
        RaycastHit hit;

        if(Physics.Raycast(headPosition.position, playerDirection, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                if(!shooting)
                {
                    StartCoroutine(shoot());
                }
                if(agent.remainingDistance < agent.stoppingDistance)
                {
                    faceplayer();
                }
                return true;
            }
        }
        return false;
    }
    void faceplayer()
    {
        Quaternion spin = Quaternion.LookRotation(playerDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation , spin, Time.deltaTime * spintarget);
    }

    public void Damage(int amount) //check
    {
        HP -= amount;
        StartCoroutine(DamageFeedback());
        if (HP <= 0)
        {
            if(mySpawner != null)
                mySpawner.DeadUpdate();
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
        Instantiate(bullet, ShootPos2.position, transform.rotation);
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
