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
    [SerializeField] int spintarget; //check
    [SerializeField] GameObject bullet; // check
    [SerializeField] Transform headPosition; // check
    [SerializeField] int viewCone;//check
    [SerializeField] int roamdist;
    [SerializeField] int roampause;
    Vector3 playerDirection; //check
    Vector3 startPosition;
    bool playerInRange; // check
    bool shooting; //check
    bool playerdestination;
    float stopdist;
    float angleToPlayer; // check
    public EnemySpawners mySpawner;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.UpdateGameGoal(1);
        startPosition = transform.position;
        stopdist = agent.stoppingDistance;
    }

    // Update is called once per frame4
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            if (playerInRange && !canSeePlayer()) //check
            {
                StartCoroutine(Roam());
            }
            else if (!playerInRange)
            {
                StartCoroutine(Roam());
            }
        }

    }

    bool canSeePlayer() //check
    {
        playerDirection = GameManager.instance.player.transform.position - headPosition.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        Debug.DrawRay(headPosition.position, playerDirection);
        Debug.Log(angleToPlayer);
        RaycastHit hit;

        if (Physics.Raycast(headPosition.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {

                agent.SetDestination(GameManager.instance.player.transform.position); //check

                if (!shooting)
                {
                    StartCoroutine(shoot());
                }
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceplayer();
                }
                agent.stoppingDistance = stopdist;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    IEnumerator Roam()
    {
        if (agent.remainingDistance < 0.05f && !playerdestination)
        {
            playerdestination = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roampause);
            Vector3 posinrange = Random.insideUnitSphere * roamdist;
            posinrange += startPosition;
            NavMeshHit hit;
            NavMesh.SamplePosition(posinrange, out hit, roamdist, 1);
            agent.SetDestination(hit.position);
            playerdestination = false;
        }
    }
    void faceplayer()
    {
        Quaternion spin = Quaternion.LookRotation(new Vector3(playerDirection.x, transform.position.y, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, spin, Time.deltaTime * spintarget);
    }

    public void Damage(int amount) //check
    {
        HP -= amount;
        StartCoroutine(DamageFeedback());
        if (HP <= 0)
        {
            if (mySpawner != null)
                mySpawner.DeadUpdate();
            GameManager.instance.UpdateGameGoal(-1);
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
            agent.stoppingDistance = 0;
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
