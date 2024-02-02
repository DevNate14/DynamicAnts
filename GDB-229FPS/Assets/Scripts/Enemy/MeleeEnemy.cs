using System.Collections;
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
    [SerializeField] float howlong;
    bool InMeleeRange;
    bool insidesphere, hasTriggered;
    bool enteredSphere;
    bool isdying;
    public EnemySpawners mySpawner;
    public AudioSource source;
    public AudioClip biteaud;
    public AudioClip deadaud;
    void Start()
    {
        if (agent == null || model == null)
        {
            return;
        }
        agent.SetDestination(transform.position);
    }
    void Update()
    {
        if (agent == null)
        {
            return;
        }
        float animationspeed = agent.velocity.normalized.magnitude;
        animate.SetFloat("Speed", Mathf.Lerp(animate.GetFloat("Speed"), animationspeed, Time.deltaTime * animationspeedtransition));
        if (insidesphere && isdying != true)
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
        StartCoroutine(Targetplayer());
        StartCoroutine(DamageFeedback());
        if (HP <= 0 && !hasTriggered)
        {
            isdying = true;
            agent.SetDestination(transform.position);
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
        if (other.CompareTag("Player"))
        {
            enteredSphere = true;
            insidesphere = true;
        }
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
        yield return new WaitForSecondsRealtime(0.2f);
        model.material.color = Temp;
    }
    IEnumerator DeadAnim()
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        yield return new WaitForSecondsRealtime(1.0f);
        source.PlayOneShot(deadaud, 1.5f);
        yield return new WaitForSecondsRealtime(0.9f);
        Destroy(gameObject);
    }
    IEnumerator MeleeDamage(float time)
    {
        InMeleeRange = true;
        animate.SetTrigger("Hit");
        Instantiate(bite, BitePos.position, transform.rotation);
        source.PlayOneShot(biteaud, 1.5f);
        yield return new WaitForSecondsRealtime(time);
        InMeleeRange = false;
    }
    IEnumerator Targetplayer()
    {
        insidesphere = true;
        yield return new WaitForSecondsRealtime(howlong);
        if (!enteredSphere)
        {
            insidesphere = false;
        }
    }
}
