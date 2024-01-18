using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform position1;
    [SerializeField] private Transform position2;
    [SerializeField] private float travelSpeed;
    [SerializeField] private float travelTime;

    private Vector3 currentDestination;
    private bool firstPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        movingTo();
    }

    // Update is called once per frame
    void Update()
    {
        platform.position = Vector3.MoveTowards(platform.position, currentDestination, travelSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }

    }

    private void movingTo(){
        if(firstPosition){
            currentDestination = position1.position;
            firstPosition = !firstPosition;
        }
        else{
            currentDestination = position2.position;
            firstPosition = !firstPosition;

        }
        Invoke("movingTo", travelTime);
    }
}
