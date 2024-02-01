using System.Collections;
using UnityEngine;

public class Emitter : MonoBehaviour
{
    [SerializeField] float shootrate;
    [SerializeField] Transform ShootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] bool Audio;
    [SerializeField] AudioClip Sound;
    [SerializeField] AudioSource Aud;
    bool shooting;
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
        if (Audio)
            Aud.PlayOneShot(Sound);
        yield return new WaitForSecondsRealtime(shootrate);
        shooting = false;
    }
}
