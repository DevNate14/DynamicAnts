using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            //win check 
            //Debug.Log("Stepped on");

            //Have WinMenu BG Appear
            GameManager.instance.CheckWinState();
        }
    }
}
