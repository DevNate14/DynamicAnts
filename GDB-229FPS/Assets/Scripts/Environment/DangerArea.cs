using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerArea : MonoBehaviour
{
    [SerializeField] int damageOverTime;
    int delayDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    void OnTriggerStay(Collider other)
    {
        if(other.isTrigger){
            return;
        }

        IDamageable thing = other.GetComponent<IDamageable>();
        if(thing != null){
            thing.Damage(damageOverTime);
        }
    }
}
