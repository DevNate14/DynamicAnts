using System.Collections;
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider explodeRadius;
    [SerializeField] int damageAmount, speed, timer, impulseAmount;
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip explosion;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.forward * speed;
        ExplodeCheck();
        src.PlayOneShot(explosion,.5f);
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
            thing.Damage(damageAmount);
            if (!other.CompareTag("Player"))
            {
                GameManager.instance.DisplayDamageDone(damageAmount);
            }
        }
        IImpluse obj = other.GetComponent<IImpluse>();
        if (obj != null) {
            Vector3 imp =(-1 * (other.transform.forward * impulseAmount) + (other.transform.up * (impulseAmount)));
            obj.AddImpluse(imp, .5f);
        }

    }


}
