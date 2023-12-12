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
    [SerializeField] int dmg;
    [SerializeField] float animationspeedtransition;
    [SerializeField] float DamageCoolDown;
    [SerializeField] float MeleeRange;
    bool InMeleeRange;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float animationspeed = agent.velocity.normalized.magnitude;
        animate.SetFloat("Speed", Mathf.Lerp(animate.GetFloat("Speed"), animationspeed, Time.deltaTime * animationspeedtransition));
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
    IEnumerator DamageFeedback() //Check
    {
        Color Temp = model.material.color;
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        model.material.color = Temp;
    }
    IEnumerator MeleeDamage(float time)
    {
        InMeleeRange = true;
        animate.SetTrigger("Hit");
        GameManager.instance.player.GetComponent<Controller>().Damage(dmg);
        yield return new WaitForSeconds(time);
        InMeleeRange = false;
    }
}