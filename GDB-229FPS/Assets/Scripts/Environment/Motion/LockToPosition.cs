using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockToPosition : MonoBehaviour
{
    Vector3 Pos;
    // Start is called before the first frame update
    void Start()
    {
        Pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Pos;
    }
}
