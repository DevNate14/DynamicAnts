using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider explodeRadius;
    [SerializeField] int damageAmount, speed, timer, impulseAmount;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        ExplodeCheck();
        Destroy(gameObject, 2);
    }
    IEnumerator ExplodeCheck() {
        yield return new WaitForSeconds(timer);
        rb.velocity = Vector3.zero;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger){
            return;
        }

        IDamageable thing = other.GetComponent<IDamageable>();
        if(thing != null){
            float mod = Vector3.Magnitude(Vector3.Normalize(other.transform.position - explodeRadius.center));
            thing.Damage((int)mod * damageAmount + damageAmount);
        }
        IImpluse obj = other.GetComponent<IImpluse>();
        if (obj != null) {
            Vector3 imp =(((-1)*(other.transform.forward * impulseAmount)) + (other.transform.up * (impulseAmount*2)));
            obj.AddImpluse(imp, .5f);
        }

    }


}
