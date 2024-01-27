using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private new GameObject light;
    private GameObject player;
    [SerializeField] GameObject SoundObject;
    [Range(0, 1)] [SerializeField] int type;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        light = transform.GetChild(0).gameObject;
        switch (type) //we can add more here later but last powerups were of no use in current context of project.
        {
            default:
            case 1:
                light.GetComponent<Light>().color = Color.red;
                break;
        }
    }
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var playCont = player.GetComponent<RigidPlayer>();
            if (playCont.CanHeal()) {
                playCont.ApplyBuff(type);
                if (SoundObject != null)
                    Instantiate(SoundObject, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}