using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private new GameObject light;
    private float rotation = 1;
    private bool up;
    private GameObject player;
    [Range(0, 3)] [SerializeField] int type;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        light = transform.GetChild(0).gameObject;
        switch (type)
        {
            case 1:
                light.GetComponent<Light>().color = Color.green;
                break;
            case 2:
                light.GetComponent<Light>().color = Color.blue;
                break;
            case 3:
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
        //if (Time.deltaTime > 0) 
            //Movement();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<Controller>().ApplyBuff(type);
            Destroy(gameObject);
        }
    }
    void Movement()
    {
        if (!up)
            transform.localPosition += new Vector3(0, 0.0015f, 0);
        else if (up)
            transform.localPosition -= new Vector3(0, 0.0015f, 0);
        else
            transform.localPosition += new Vector3(0, 0.0015f, 0);
        if ((up && transform.localPosition.y >= 0.25f) || (!up && transform.localPosition.y <= -0.15f))
            up = !up;
        transform.Rotate(0, rotation, 0);
    }
}