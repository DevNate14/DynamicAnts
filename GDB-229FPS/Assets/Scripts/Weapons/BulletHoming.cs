using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoming : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    private Transform destination;

    Vector3 direction;
    Quaternion rotation;

    [SerializeField] int damageAmount;
    [SerializeField] int destroyTimer;
    [SerializeField] int speed;
    [SerializeField] float turnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTimer);
    }

    private void Update()
    {
        if (destination != null)
        {
            rb.velocity = transform.forward * speed;
            direction = destination.position - rb.position;
            Debug.DrawRay(rb.position, direction, Color.blue);
            rotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, rotation, turnSpeed * Time.deltaTime));
        }
    }

    public void SetDestination(Transform destination)
    { 
        this.destination = destination; 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger){
            return;
        }

        IDamageable thing = other.GetComponent<IDamageable>();
        if(thing != null){
            thing.Damage(damageAmount);
        }
        Destroy(gameObject);
    }


}
