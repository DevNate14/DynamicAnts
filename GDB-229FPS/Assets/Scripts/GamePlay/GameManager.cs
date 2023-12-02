using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject Player;
    public Movement PlayerScript;
    public float Gravity;
    void Awake() {
        instance = this;
        Player = GameObject.FindWithTag("Player");
        PlayerScript = GetComponent<Movement>();
        //Gravity = PlayerScript.GetGravity();
    }
    void Update()
    {
        
    }
}
