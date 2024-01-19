using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private new GameObject light;
    private float rotation = 1;
    private bool up;
    private GameObject player;
    [Range(0, 1)] [SerializeField] int type;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        light = transform.GetChild(0).gameObject;
        switch (type)
        {
            //case 1:
            //    light.GetComponent<Light>().color = Color.green;
            //    break;
            //case 2:
            //    light.GetComponent<Light>().color = Color.blue;
            //    break;
            case 1:
                light.GetComponent<Light>().color = Color.red;
                break;
            default:
                light.GetComponent<Light>().color = Color.white;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<Controller>().ApplyBuff(type);
            Destroy(gameObject);
        }
    }
}