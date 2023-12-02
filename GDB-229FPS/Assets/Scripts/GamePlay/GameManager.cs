using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public Movement playerScript;
    public float gravity;
    int enemiesRemaining;
    public bool isPaused;

    [SerializeField] TMP_Text enemyCountText;


    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;




    void Awake() {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = GetComponent<Movement>();
        //Gravity = PlayerScript.GetGravity();
    }
    void Update() {
        
    }

    public void StatePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0; //Pauses everything- Minus menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

     public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemyCountText.text = enemiesRemaining.ToString("0");

        if (enemiesRemaining <= 0)
        {
            //You win!
            StatePaused();
            menuActive = menuWin;
            menuActive.SetActive(true);
            
        }
     }

}
