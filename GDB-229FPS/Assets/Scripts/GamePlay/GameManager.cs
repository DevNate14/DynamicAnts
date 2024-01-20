using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections;
using MiscUtil.Extensions.TimeRelated;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("------------------------------ PLAYER ------------------------------\n")]
    [SerializeField] TMP_Text totalDamage;
    int damageDone;
    [SerializeField] TMP_Text playerHPMissing;
    [SerializeField] TMP_Text playerHPTotal;
    public Image playerHPBar;
    public GameObject player;
    public GameObject playerSpawnPOS;
    public Controller playerScript;
    //int playerHP;

    [Header("------------------------------ ENEMY------------------------------\n")]
    [SerializeField] TMP_Text enemyCountText;
    int enemiesRemaining;

    [Header("------------------------------ GUNS ------------------------------\n")]

    [SerializeField] TextMeshProUGUI ammoSizeText;
    [SerializeField] TextMeshProUGUI ammoCountText;
    [SerializeField] RawImage weaponIcon;

    [Header("------------------------------ MENUS ------------------------------\n")]
    [SerializeField] bool isTitleScreen;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    public bool isPaused;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject reloadMessage;
    [SerializeField] GameObject instructionsPage;
    [SerializeField] GameObject optionsPage;

    [Header("------------------------------ GAME DIALOG ------------------------------\n")]
    //[SerializeField] private string gameText;
    // [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] GameObject dialoguePanel;
    float charactersPerSecond = 8.5f;



    [Header("------------------------------ OTHER ------------------------------\n")]
    public GameObject playerDamageScreen;
    public Camera playerCam;
    float timeScaleOrig;
    public float gravity;
    public GameObject LoadingScreen;

    void Awake()
    {
        instance = this;
        if (!isTitleScreen)
        {
            playerSpawnPOS = GameObject.FindWithTag("Respawn");
            
            player = GameObject.FindWithTag("Player");
            //playerScript = player.GetComponent<Controller>();
            playerCam = FindObjectOfType<Camera>();
            //gravity = playerScript.GetGravity();
            timeScaleOrig = Time.timeScale;
        }

        TriggerDialogue("WELCOME TO YOUR FIRST MISSION! GRAB THE GUNS, AND SHOOT THE ENEMIES!");
    }

    void Update()
    {
        if (!isTitleScreen)
            PauseMenu();
    }

    public void TriggerDialogue(string message)
    {
        StartCoroutine(TypeText(message));
    }

    IEnumerator TypeText(string line)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = ""; //Clears Text

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(1F / charactersPerSecond);
        }

        yield return new WaitForSeconds(5F);
        dialogueText.text = ""; //Clears Text


        dialoguePanel.SetActive(false);
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
        //playerHP += amount;
    }
    public void UpdateHPBar(int hpMissing, int hpTotal)
    {
        playerHPMissing.text = hpMissing.ToString("00");
        playerHPTotal.text = hpTotal.ToString("00");
    }

    public void DisplayDamageDone(int amount) // needs a rework of damage system to track which bullets are hitting and from player unless we decouple player and enemy bullets
    {
        damageDone += amount;
        totalDamage.text = damageDone.ToString("00");
    }

    public void DisplayGunImage(Texture2D texture)
    {
        //References to Textures that are added in the GunSO on Unity
        {
            if (texture != null)
            {
                weaponIcon.texture = texture;
            }
        }
    } //Need to add code for Weapon Pick-up

    public void UpdateAmmoUI(GunStatsSO newWeapon)
    {

        ammoSizeText.text = newWeapon.ammoCount.ToString("00");
        ammoCountText.text = newWeapon.magAmmoCount.ToString("00");

        if (newWeapon.magAmmoCount == 0) //Should only call UI when mag is empty .
        {
            ReloadUI();
        }

    }

    public void ReloadUI()
    {
        StartCoroutine(ReloadUIEvent());
    }

    IEnumerator ReloadUIEvent()
    {
        reloadMessage.SetActive(true);
        yield return new WaitForSeconds(2);
        reloadMessage.SetActive(false);
        if (Input.GetButtonDown("Reload")
        && menuActive == null)
        {
            //Have Reload UI appear
            //StateUnpaused(); //Game should still play....?
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
        menuActive.GetComponentInChildren<Button>().Select();
    }


    public void YouLose()
    {
        //new WaitForSeconds(3);
        StatePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
        menuActive.GetComponentInChildren<Button>().Select();
    }

    public void Instructions()
    {
        //StatePaused();
        menuActive = instructionsPage;
        menuActive.SetActive(true);
        menuActive.GetComponentInChildren<Button>().Select();
    }
    public void Options()
    {
        menuActive = optionsPage;
        menuActive.SetActive(true);
        menuActive.GetComponentInChildren<Button>().Select();
    }

    public void PauseMenu()
    {
        if (Input.GetButtonDown("Cancel")
        && menuActive == null)
        {
            StatePaused();
            menuActive = menuPause;
            menuActive.SetActive(isPaused);
            menuActive.GetComponentInChildren<Button>().Select();
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


