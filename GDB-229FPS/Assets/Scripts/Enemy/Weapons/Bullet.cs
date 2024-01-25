using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] bool shotByPlayer;
    [SerializeField] int damageAmount;
    [SerializeField] int destroyTimer;
    [SerializeField] int speed;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTimer);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger){
            return;
        }
        IDamageable thing = other.GetComponent<IDamageable>();
        bool isPlayer = other.CompareTag("Player");
        if (thing != null){
            if (isPlayer && shotByPlayer) 
                return;
            if (!isPlayer)
                GameManager.instance.DisplayDamageDone(damageAmount);
            thing.Damage(damageAmount);
        }
        Destroy(gameObject);
    }


}
