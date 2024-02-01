using System.Collections;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] int damage;
    public bool hit;
    private void OnTriggerStay(Collider other)
    {
        if (!hit)
        {
            if (other.isTrigger)
                return;

            StartCoroutine(Contact(other)); // damages collider on a cooldown
        }
    }
    IEnumerator Contact(Collider other) 
    {
        hit = true;
        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.Damage(damage);
        }
        yield return new WaitForSecondsRealtime(cooldown);
        hit = false;
        // this most likely only can trigger on one target at a time, and I do not know how to fix that yet
    }
}
