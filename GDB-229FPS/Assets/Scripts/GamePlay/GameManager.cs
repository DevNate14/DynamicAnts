using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public GameObject playerSpawnPOS;
    public Controller playerScript;

    float timeScaleOrig;
    public float gravity;
    int enemiesRemaining;
    int playerHP;
    public bool isPaused;
    public Image playerHPBar;

    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text playerHPMissing;
    [SerializeField] TMP_Text playerHPTotal;
    [SerializeField] public TextMeshProUGUI ammoSizeText;
    [SerializeField] public TextMeshProUGUI ammoCountText;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject reloadMessage;
    public GameObject playerDamageScreen;


    void Awake() {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Controller>();
        //Gravity = PlayerScript.GetGravity();
        playerSpawnPOS = GameObject.FindWithTag("PlayerSpawnPOS");
        timeScaleOrig = Time.timeScale;

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
        
     }

     public void ReloadUI()
     {
        //Have Reload UI appear
        StateUnpaused(); //Game should still play....?
        menuActive = reloadMessage;
        menuActive.SetActive(true);
     }

    public void CheckWinState()
    {
       if (enemiesRemaining <= 0)
        {
            //You win!
            // StatePaused();
            // menuActive = menuWin;
            // menuActive.SetActive(true);
           YouWin();
        }
    }

    public void YouWin()
    {
        StatePaused();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

     public void YouLose()
    {
        StatePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
}


