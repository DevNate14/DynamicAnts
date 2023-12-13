using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PogoEnemy : MonoBehaviour, IDamageable
{
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] float shootrate;
    [SerializeField] Transform ShootPos;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent; 
    [SerializeField] GameObject bullet; 
    //[SerializeField] Transform headPosition;
    [SerializeField] int viewCone;
    [Range(10, 15)] [SerializeField] int jumpHeight;
    [SerializeField] Rigidbody rb;
    [SerializeField] int turnSpeed;
    Vector3 playerDirection;
    bool playerInRange; 
    bool shooting;
    float angleToPlayer; 
    bool jumping;
    bool isGrounded;
    Vector3 jumpVelocity;
    float yVelocity;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(IsCharacterGrounded());
        jumpVelocity = rb.velocity;

        if(playerInRange && canSeePlayer()){

        }   

        rb.velocity = jumpVelocity;
        Debug.Log(rb.velocity); 
        
    }
        

    bool canSeePlayer() 
    {
        playerDirection = GameManager.instance.player.transform.position - agent.transform.position;
        angleToPlayer = Vector3.Angle(playerDirection, agent.transform.forward);
        Debug.DrawRay(agent.transform.position, playerDirection);
        // Debug.Log(angleToPlayer);

        RaycastHit hit;

        if(Physics.Raycast(agent.transform.position, playerDirection, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(GameManager.instance.player.transform.position); 
                
                if(agent.remainingDistance < agent.stoppingDistance){
                    faceTarget();
                }
                
                return true;
            }
        }
        return false;
    }
    bool IsCharacterGrounded(){
        if(rb.transform.position.y <= agent.transform.position.y){
            isGrounded = true;
            if(!shooting)
            {
                StartCoroutine(shoot());
            }
            if(!jumping){
                StartCoroutine(jump());
            }
        }
        else{
            isGrounded = false;
        }

        return isGrounded;
    }
    IEnumerator jump(){


        jumping = true;
        rb.velocity = Vector3.up * jumpHeight;
        yield return new WaitForSeconds(.2f);

        jumping = false;
    }

    void faceTarget(){
        Quaternion rotation = Quaternion.LookRotation(new Vector3(playerDirection.x, agent.transform.position.y, playerDirection.z));
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, rotation, Time.deltaTime * turnSpeed);
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
    //    y rotation is -1 to 1
    // make a for loop to hit -1 to 1
        // Instantiate(bullet, ShootPos.position, agent.transform.rotation);
        for(float i = -1; i < 1.1; i += 0.1f){
            Instantiate(bullet, ShootPos.position, new Quaternion(ShootPos.rotation.x , i, ShootPos.rotation.z , ShootPos.rotation.w));
        }

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
