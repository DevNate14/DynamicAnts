using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour, IDamageable
{

    //PLEASE READ BEFORE YOU WORK WITH THIS CODE
    // This code is fully functional and has been tested the code is simple on purpose it is meant to be used for an enemy that will follow you it does not use
    // Ray Cast to find the player it just sets the destination and goes once it gets close enough it deals damage to the player then there is a cooldown of
    //  1-2 seconds and deals damage again if it's still within the range

    [Range(1, 10)][SerializeField] int HP; //check
    [SerializeField] Renderer model; //check
    [SerializeField] NavMeshAgent agent; // check
    [SerializeField] Animator animate;
    [SerializeField] GameObject bite;
    [SerializeField] Transform BitePos;
    [SerializeField] float animationspeedtransition;
    [SerializeField] float DamageCoolDown;
    [SerializeField] float MeleeRange;
    bool InMeleeRange;
    bool insidesphere,hasTriggered;
    public EnemySpawners mySpawner;
    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        float animationspeed = agent.velocity.normalized.magnitude;
        animate.SetFloat("Speed", Mathf.Lerp(animate.GetFloat("Speed"), animationspeed, Time.deltaTime * animationspeedtransition));
        if (insidesphere)
        {
            agent.SetDestination(GameManager.instance.player.transform.position); //check
            if (agent.remainingDistance < MeleeRange)
            {
               if (!InMeleeRange)
                {
                    StartCoroutine(MeleeDamage(DamageCoolDown));
                }
                //Damage Player and add cooldown to the damage
            }
        }
        else
        {
            agent.SetDestination(transform.position);
        }
    }

    public void Damage(int amount) //check
    {
        HP -= amount;
        StartCoroutine(DamageFeedback());
        if (HP <= 0 && !hasTriggered)
        {
            hasTriggered = true;
            if (mySpawner != null)
                mySpawner.DeadUpdate();
            animate.SetBool("Dead", true);
            StartCoroutine(DeadAnim());
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
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            insidesphere = true;
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            insidesphere = false;
    }
        
    IEnumerator DamageFeedback() //Check
    {
        Color Temp = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Temp;
    }
    IEnumerator DeadAnim()
    {
        yield return new WaitForSeconds(1.9f);
        Destroy(gameObject);
    }
    IEnumerator MeleeDamage(float time)
    {
        InMeleeRange = true;
        animate.SetTrigger("Hit");
        Instantiate(bite, BitePos.position, transform.rotation);
        yield return new WaitForSeconds(time);
        InMeleeRange = false;
    }
}
