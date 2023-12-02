using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] float shootrate;
    [SerializeField] Transform ShootPos;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform headPosition;
    [SerializeField] int viewCone;
    Vector3 playerDirection;
    bool playerInRange;
    bool shooting;
    float angleToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame4
    void Update()
    {
        if(playerInRange && canSeePlayer()){

        }   
    }

    bool canSeePlayer(){
        playerDirection = GameManager.instance.player.transform.position - headPosition.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);

        RaycastHit hit;
        if(Physics.Raycast(headPosition.position, playerDirection, out hit)){
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewCone){
                
                agent.SetDestination(GameManager.instance.player.transform.position);

                if(!shooting){
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        return false;
    }


    public void Damage(int amount)
    {
        HP -= amount;
        StartCoroutine(DamageFeedback());
        if (HP <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.UpdateGameGoal(-1);
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
    
    IEnumerator shoot()
    {
       shooting = true;
       Instantiate(bullet, ShootPos.position, transform.rotation);
       yield return new WaitForSeconds(shootrate);

       shooting = false;
    }
    
    IEnumerator DamageFeedback()
    {
        Color Temp = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Temp;
    }
}
