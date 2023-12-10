using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public Movement playerScript;

    float timeScaleOrig;
    public float gravity;
    int enemiesRemaining;
    int playerHP;
    public bool isPaused;
    public Image playerHPBar;

    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text playerHPTotal;
    [SerializeField] TMP_Text playerHPMissing;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;




    void Awake() {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Movement>();
        //Gravity = PlayerScript.GetGravity();
    }
    
    void Update() 
    {
        if (Input.GetButtonDown("Cancel") 
        && menuActive == null)
        {
            StatePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }
    }

    public void StatePaused()
    {
        isPaused = !isPaused;
        Time.timeScale = 0; //Pauses everything- Minus menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StateUnpaused()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

     public void UpdateGameGoal(int amount)
    {
        enemiesRemaining += amount;
        enemyCountText.text = enemiesRemaining.ToString("00");

        //This should be able to call Player HP 
        // Should be HP # Missing/ HP # Total 
        playerHP += amount;
        playerHPMissing.text = playerHP.ToString("00"); 
        playerHPTotal.text = playerHP.ToString("00");
        


        if (enemiesRemaining <= 0)
        {
            //You win!
            StatePaused();
            menuActive = menuWin;
            menuActive.SetActive(true);
            
        }
     }

     public void YouLose()
    {
        StatePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}


