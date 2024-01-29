using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject trap;
    [SerializeField] Collider Hitbox;
    [SerializeField] bool RaycastMode;
    public bool hit;
    void Start()
    {
        if (RaycastMode)
        {
            Hitbox.enabled = false;
        }
    }
    void Update()
    {
        if (RaycastMode && !hit) //just so this triggers only once
        {
            //Debug.DrawRay(transform.position, transform.up * 1, Color.red);
            if (Physics.Raycast(transform.position, transform.up, 1))
            {
                hit = true;
                Instantiate(trap, transform.position + transform.up * 10, transform.rotation); // spawn a boulder above
                transform.position = transform.position - transform.up * 0.08f; // pressure plate sinks (a little too far) into the floor
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;
        hit = true;
        Instantiate(trap, transform.position + transform.up * 10, transform.rotation); // spawn a boulder above
        transform.position = transform.position - transform.up * 0.08f; // pressure plate sinks (a little too far) into the floor
        Hitbox.enabled = false;
    }
}
