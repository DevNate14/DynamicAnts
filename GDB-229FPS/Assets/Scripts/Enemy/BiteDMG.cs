using UnityEngine;

public class BiteDMG : MonoBehaviour
{
    [SerializeField] int damageAmount;
    [SerializeField] float destroyTimer;
    [SerializeField] int impulseamount;
    IImpluse impulse;
    void Start()
    {
        Destroy(gameObject, destroyTimer);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        impulse = other.GetComponent<IImpluse>();
        IDamageable thing = other.GetComponent<IDamageable>();
        if (thing != null)
        {
            thing.Damage(damageAmount);
            PushPlayer(impulseamount);
        }
        Destroy(gameObject);
    }
    public void PushPlayer(int push)
    {
        if (impulse != null)
        {        
            Vector3 direction = (-1 * GameManager.instance.player.transform.forward + (GameManager.instance.player.transform.up * (impulseamount)));
            impulse.AddImpluse(direction, 0.5f);
        }
    }
}
