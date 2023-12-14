using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using MiscUtil.Extensions.TimeRelated;

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
    int damageDone;
    public bool isPaused;
    public Image playerHPBar;
    public Texture2D weaponIcon;

    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text playerHPMissing;
    [SerializeField] TMP_Text playerHPTotal;
    [SerializeField] public TextMeshProUGUI ammoSizeText;
    [SerializeField] public TextMeshProUGUI ammoCountText;
    [SerializeField] TMP_Text totalDamage;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject reloadMessage;
    [SerializeField] GameObject instructionsPage;
    public GameObject playerDamageScreen;


    void Awake()
    {
        instance = this;
        playerSpawnPOS = GameObject.FindWithTag("Respawn");
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<Controller>();
        gravity = playerScript.GetGravity();
        timeScaleOrig = Time.timeScale;
    }

    void Update()
    {
        PauseMenu();
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

    public void DisplayDamageDone(int amount)
    {
        damageDone += amount;
        totalDamage.text = damageDone.ToString("00");
    }

    public void DisplayGunImage(Texture2D texture)
    {
        //References to Textures that are added in the GunSO on Unity
        {
            if (weaponIcon != null && texture != null)
            {
                weaponIcon = texture;
            }
        }
    } //Need to add code for Weapon Pick-up


    public void ReloadUI()
    {

        if (Input.GetButtonDown("Reload")
        && menuActive == null)
        {
            //Have Reload UI appear
            StateUnpaused(); //Game should still play....?
            menuActive = reloadMessage;
            menuActive.SetActive(true);
        }

        else if (Input.GetButtonDown("Reload") 
        && menuActive != null)
        {
            menuActive.SetActive(false); 
            //Reload UI should be gone, after Player clicks R
        }
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
        //new WaitForSeconds(3);
        StatePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
    
    public void Instructions()
    {
        StatePaused();
        menuActive = instructionsPage;
        menuActive.SetActive(true);
    }

    public void PauseMenu()
    {
        if (Input.GetButtonDown("Cancel")
        && menuActive == null)
        {
            StatePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
        }

        // else if (Input.GetButtonDown("Cancel")
        // && menuActive != null)
        // {
        //     StatePaused();
        //     menuActive = menuPause;
        //     menuActive.SetActive(!isPaused);
        // }
    }
}


