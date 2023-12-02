using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] float cooldown;
    [SerializeField] int damage;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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

        yield return new WaitForSeconds(cooldown);
        hit = false;
        // this most likely only can trigger on one target at a time, and I do not know how to fix that yet
    }
}
