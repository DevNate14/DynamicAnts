using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : BreakableObject
{
    [SerializeField] GameObject Barrel;
    [SerializeField] Collider Hitbox;
    [SerializeField] Collider BlastHitbox;
    [SerializeField] GameObject BrokenBarrel;
    [SerializeField] ParticleSystem ExplosionFX;
    [SerializeField] int BlastDmg;
    [SerializeField] int ImpulseAmount;
    [SerializeField] float BlastHitboxUptime;


    public override void Break()
    {
        BrokenBarrel.SetActive(true);
        Barrel.SetActive(false);
        Hitbox.enabled = false;
        StartCoroutine(Explosion());
    }

    private IEnumerator Explosion()
    {
        Instantiate(ExplosionFX, transform.position, transform.rotation);
        BlastHitbox.enabled = true;
        yield return new WaitForSeconds(BlastHitboxUptime);
        BlastHitbox.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamageable thing = other.GetComponent<IDamageable>();
        if (thing != null)
        {
            thing.Damage(BlastDmg);
        }
        IImpluse obj = other.GetComponent<IImpluse>();
        if (obj != null)
        {
            Vector3 imp = (-1 * (other.transform.forward * ImpulseAmount) + (other.transform.up * (ImpulseAmount)));
            obj.AddImpluse(imp, .5f);
        }
    }
}
