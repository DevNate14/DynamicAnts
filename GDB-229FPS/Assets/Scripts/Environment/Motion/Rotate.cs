using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] bool WithObject;
    [SerializeField] Transform Object;
    [SerializeField] bool WithDelay;
    [SerializeField] float Delay;
    [Range(0, 1)][SerializeField] int DelayType; //1 for rotating with an object
    [SerializeField] float LerpSpeed;
    [SerializeField] Quaternion Offset;
    Quaternion Rotation;
    bool CoroutineRunning;

    [SerializeField] float X;
    [SerializeField] float Y;
    [SerializeField] float Z;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!WithDelay)
        {
            if (WithObject)
            {
                transform.rotation = Object.rotation;
            }
            else
                transform.Rotate(X * Time.deltaTime, Y * Time.deltaTime, Z * Time.deltaTime);
        }
        else
        {
            if (CoroutineRunning)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Rotation * Offset, LerpSpeed * Time.deltaTime);
            }
            else
            {
                StartCoroutine(RotateDelay());
            }
        }
    }

    IEnumerator RotateDelay()
    {
        CoroutineRunning = true;
        yield return new WaitForSeconds(Delay);
        if (WithObject)
        {
            switch (DelayType)
            {
                case 0:
                    transform.rotation = Object.rotation;
                    break; 
                case 1:
                    Rotation = Object.rotation;
                    break;

            }
        }
        else
            transform.Rotate(X, Y, Z);
        CoroutineRunning = false;
    }
}
