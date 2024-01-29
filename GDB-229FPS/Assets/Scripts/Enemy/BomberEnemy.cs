using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BomberEnemy : MonoBehaviour
{
    [Range(1, 10)][SerializeField] int HP;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent; 
    [SerializeField] int viewCone;
    [SerializeField] int turnSpeed;
    [SerializeField] int damageAmount;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;
    List<Collider> inDanger = new List<Collider>();
    Vector3 playerDirection;
    bool playerInRange; 
    float angleToPlayer; 
    bool timerRunning;
    float detonateTimer;
    bool destinationChosen;
    float stoppingDistanceOriginal;
    Vector3 startingPosition;
    void Start()
    {
        detonateTimer = 0f;
        startingPosition = transform.position;
        stoppingDistanceOriginal = agent.stoppingDistance;
    }
    void Update()
    {
        runTimer();

        if(agent.isActiveAndEnabled){

            if(playerInRange && !canSeePlayer()){
                StartCoroutine(roam());
            }
            else if(!playerInRange){
                StartCoroutine(roam());
            }
        }
    }
        

    bool canSeePlayer() 
    {
        playerDirection = GameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        //Debug.DrawRay(transform.position, playerDirection);
        //Debug.Log(angleToPlayer);
        //Debug.DrawRay(transform.position, playerDirection);
        
        RaycastHit hit;

        if(Physics.Raycast(agent.transform.position, playerDirection, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                timerRunning = true;
                agent.SetDestination(GameManager.instance.player.transform.position); 
                
                if(agent.remainingDistance < agent.stoppingDistance){
                    faceTarget();
                }
                agent.stoppingDistance = stoppingDistanceOriginal;
                return true;
            }
        }
        return false;
    }

    IEnumerator roam(){
        if(agent.remainingDistance < .05f && !destinationChosen){
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);

            Vector3 randomPosition = Random.insideUnitSphere * roamDistance;
            randomPosition += startingPosition;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPosition, out hit, roamDistance, 1);
            agent.SetDestination(hit.position);

            destinationChosen = false;
        }
            Debug.Log(agent.destination);
    }
    
    void faceTarget(){
        Quaternion rotation = Quaternion.LookRotation(new Vector3(playerDirection.x, transform.position.y, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }


public void Damage(int amount)
    {
        HP -= amount;
        StartCoroutine(DamageFeedback());
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

    public void OnTriggerEnter(Collider other)
    {
        if(!other.isTrigger){
            inDanger.Add(other);
        }
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerStay(Collider other)
    {

        if(detonateTimer >= 5f){
            // go boom
            //Debug.Log("BOOOOOOOOOOMMMMMM");
            death();
        }
    }
    void runTimer(){
        if(timerRunning){
            detonateTimer += Time.deltaTime;
        }
    }
    void death(){
        IDamageable item;
        foreach(Collider itemCollider in inDanger){
            item = itemCollider.GetComponent<IDamageable>();
            if(item != null){
                item.Damage(damageAmount);
            }
        }
        Destroy(gameObject);
    }
    public void OnTriggerExit(Collider other)
    {
        inDanger.Remove(other);

        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            timerRunning = false;
            detonateTimer = 0f;
            agent.stoppingDistance = 0;
        }

        
    }

    void Explode(){
        
    }
    
    
    IEnumerator DamageFeedback() 
    {
        Color Temp = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Temp;
    }
}
