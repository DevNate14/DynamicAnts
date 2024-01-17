using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
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
        transform.Rotate(X * Time.deltaTime, Y * Time.deltaTime, Z * Time.deltaTime);
    }
}
