using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] GameObject trap;
    [SerializeField] Collider Hitbox;
    [SerializeField] bool RaycastMode;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        if (RaycastMode)
        {
            Hitbox.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RaycastMode && !hit) //just so this triggers only once
        {
            Debug.DrawRay(transform.position, transform.up * 1, Color.red);
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
        hit = true;
        Instantiate(trap, transform.position + transform.up * 10, transform.rotation); // spawn a boulder above
        transform.position = transform.position - transform.up * 0.08f; // pressure plate sinks (a little too far) into the floor
    }
}
