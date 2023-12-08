using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingFlap : MonoBehaviour
{
    [SerializeField] float FlapHeight;
    [SerializeField] float FlapTime;
    private bool MovingUp;
    private float ElapsedTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime += Time.deltaTime;
        if (ElapsedTime <= FlapTime)
        {
            if (MovingUp)
                transform.Translate(Vector3.up * Time.deltaTime * FlapHeight / FlapTime, Space.Self);
            else
                transform.Translate(-Vector3.up * Time.deltaTime * FlapHeight / FlapTime, Space.Self);
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition, Time.deltaTime / 100);
            MovingUp = !MovingUp;
            ElapsedTime = 0;
        }
    }
}
