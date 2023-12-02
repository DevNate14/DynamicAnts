using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    bool isShooting;
    [SerializeField] GameObject shot;
    [SerializeField] float cooldown;
    [SerializeField] Transform shootPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShooting)
            StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(shot, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(cooldown);

        isShooting = false;
    }
}
