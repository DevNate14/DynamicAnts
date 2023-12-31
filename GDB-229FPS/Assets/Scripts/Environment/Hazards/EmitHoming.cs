using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitHoming : MonoBehaviour
{
    [SerializeField] float shootrate;
    [SerializeField] float destroyTime; // 0 for infinite
    [SerializeField] Transform ShootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] BulletHoming script;
    [SerializeField] Transform destination;
    [SerializeField] bool targetPlayer;
    bool shooting;
    // Start is called before the first frame update
    void Start()
    {
        if (targetPlayer)
        {
            destination = GameManager.instance.player.transform;
        }
        if (destroyTime > 0)
        {
            Destroy(gameObject, destroyTime);
        }
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
        GameObject b = Instantiate(bullet, ShootPos.position, transform.rotation); //i figured it out
        script = b.GetComponent<BulletHoming>();
        script.SetDestination(destination);
        yield return new WaitForSeconds(shootrate);

        shooting = false;
    }
}
