using UnityEngine;

public class BoulderTrap : MonoBehaviour
{
    [SerializeField] Collider col;
    // amount of damage it should deal
    [SerializeField] int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.Damage(damage);
        }
        col.isTrigger = false;
    }
}
