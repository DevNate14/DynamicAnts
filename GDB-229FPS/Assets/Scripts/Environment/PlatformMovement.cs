using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform position1;
    [SerializeField] private Transform position2;
    private Vector3 pos1;
    private Vector3 pos2;

    // Start is called before the first frame update
    void Start()
    {
        pos1 = position1.position;
        pos2 = position2.position;
    }

    // Update is called once per frame
    void Update()
    {
        platform.position = Vector3.Lerp(pos1, pos2, 3f);
    }
}
