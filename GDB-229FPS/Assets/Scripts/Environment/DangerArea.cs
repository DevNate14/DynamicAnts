using UnityEngine;

public class DangerArea : MonoBehaviour
{
    [SerializeField] int damageOverTime;
    //int delayDamage;
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
