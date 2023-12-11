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
        ExplodeCheck();
        Destroy(gameObject, 2);
    }
    IEnumerator ExplodeCheck() {
        rb.velocity = transform.forward * speed;
        yield return new WaitForSeconds(timer);
        rb.velocity = Vector3.zero;
        explodeRadius.enabled = true;
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
            Vector3 imp = -1 * (other.transform.forward * (impulseAmount * Vector3.Magnitude(other.transform.position - explodeRadius.center)) /*+ other.transform.up */);
            //may need to come back and change this but need to play with the bullet before I know if this is right or not
            obj.AddImpluse(imp, .5f);
        }
    }


}
