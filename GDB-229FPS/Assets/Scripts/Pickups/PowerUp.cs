using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private GameObject light;
    private float elapsedTime;
    private float watchTime = 8;
    private float rotation = 1;
    private float movement = 0.01f;
    private float moveDiff = 0.02f;
    private bool up = true;
    bool triggerSet;
    private GameObject player;
    [Range(0, 3)] [SerializeField] int type;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        light = GameObject.Find("PowerUpLight");
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
        if (Time.deltaTime > 0)
            Movement();
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
        if (up)
            transform.position += new Vector3(0, 0.0015f, 0);
        else if (!up)
            transform.position -= new Vector3(0, 0.0015f, 0);
        else
            transform.position += new Vector3(0, 0.0015f, 0);
        if ((up && transform.position.y >= 0.25f) || (!up && transform.position.y <= 0))
            up = !up;
        transform.Rotate(0, rotation, 0);
    }
    bool InRange(Vector3 pos)
    {
        return (transform.position.x - 1 >= pos.x || transform.position.x + 1 <= pos.x && transform.position.z - 1 >= pos.z || transform.position.z + 1 <= pos.z) ;
    }
}
/*        Animate();
        if (InRange(player.transform.position))
        {
            player.GetComponent<Controller>().ApplyBuff(type);
            Destroy(gameObject);
        }
    }
    IEnumerator Animate()
{
    if (rotation == 360)
        rotation = 0;
    ++rotation;
    transform.Rotate(0, rotation, 0);
    if (elapsedTime != watchTime)
        ++elapsedTime;
    else
    {
        elapsedTime = 0;
        moveDiff *= -1;
    }
    movement += moveDiff;
    transform.position += new Vector3(0, movement, 0);
    yield return new WaitForSeconds(1);
}
*/