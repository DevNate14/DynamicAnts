using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    [SerializeField] float shootrate;
    [SerializeField] Transform ShootPos;
    [SerializeField] GameObject bullet;
    bool shooting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!shooting)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        shooting = true;
        Instantiate(bullet, ShootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootrate);

        shooting = false;
    }
}
