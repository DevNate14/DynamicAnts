using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [SerializeField] Transform Center;
    [SerializeField] float Speed;
    [SerializeField] bool AroundX;
    [SerializeField] bool AroundY;
    [SerializeField] bool AroundZ;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // this was inspired by code i found through google, but i understand how it works so its okay
        //if (AroundX)
        //    transform.position = Quaternion.Euler(Speed * Time.deltaTime, 0, 0) * (transform.position - Center.position) + Center.position;
        //if (AroundY)
        //    transform.position = Quaternion.Euler(0, Speed * Time.deltaTime, 0) * (transform.position - Center.position) + Center.position;
        //if (AroundZ)
        //    transform.position = Quaternion.Euler(0, 0, Speed * Time.deltaTime) * (transform.position - Center.position) + Center.position;
        // this is the same code but neater
        transform.position = Quaternion.Euler(
            AroundX ? Speed * Time.deltaTime : 0,
            AroundY ? Speed * Time.deltaTime : 0,
            AroundZ ? Speed * Time.deltaTime : 0) 
            * (transform.position - Center.position) + Center.position;
        // this code currently only works as intended if 
        // 1. you have only one of the bools set to true at a time
        //     - if you have two set at the same time it might work correctly in some cases
        // 2. both axes you aren't rotating around aren't set to zero at the same time (relative to the center point)
        //     - example: if you rotate a child around the X of its parent and local position is (3, 0, 0) it will simply not move
        // 3. either the center is stationary or the orbiting object is a child of it
        //     - this rule was a jumpscare so there are probably more rules i was tricked into not finding

    }
}
