using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private new GameObject light;
    private GameObject player;
    [Range(0, 1)] [SerializeField] int type;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        light = transform.GetChild(0).gameObject;
        switch (type) //we can add more here later but last powerups were of no use in current context of project.
        {
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
            player.GetComponent<RigidPlayer>().ApplyBuff(type);
            Destroy(gameObject);
        }
    }
}