using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    [SerializeField] int timer;
    [SerializeField] int impulseamount;
    IImpluse impulse;
    bool jumping;
    bool insidebox;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (insidebox)
        {
            PushPlayerUp(impulseamount);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (other.CompareTag("Player"))
            insidebox = true;
        impulse = other.GetComponent<IImpluse>();
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            insidebox = false;
        impulse = null;
    }
    public void PushPlayerUp(int push)
    {

        if (impulse != null)
        {
            Vector3 direction = (-1 * GameManager.instance.player.transform.forward + (GameManager.instance.player.transform.up * (impulseamount)));
            impulse.AddImpluse(direction, 0.5f);
        }
    }
}
