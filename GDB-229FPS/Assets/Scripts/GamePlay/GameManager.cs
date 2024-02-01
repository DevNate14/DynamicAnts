using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static PersistenceManager persistenceManager;

    [Header("------------------------------ PLAYER ------------------------------\n")]
    [SerializeField] TMP_Text totalDamage;
    int damageDone;
    [SerializeField] TMP_Text playerHPMissing;
    [SerializeField] TMP_Text playerHPTotal;
    public Image playerHPBar;
    public GameObject player;
    public GameObject playerSpawnPOS;
    public RigidPlayer playerScript;
    public Inventory playerInventory;

    [Header("------------------------------ GUNS ------------------------------\n")]

    [SerializeField] TextMeshProUGUI ammoSizeText;
    [SerializeField] TextMeshProUGUI ammoCountText;
    [SerializeField] RawImage weaponIcon;

    [Header("------------------------------ MENUS ------------------------------\n")]
    [SerializeField] bool isTitleScreen;
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    public bool isPaused;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject reloadMessage;
    [SerializeField] GameObject instructionsPage;
    [SerializeField] GameObject optionsPage;

    [Header("------------------------------ GAME DIALOGUE ------------------------------\n")]
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] GameObject dialoguePanel;
    float charactersPerSecond = 8.5f;
    private Queue<string> dialogueQueue = new Queue<string>();
    private bool isDialoguePlaying = false;

    [Header("------------------------------ KEY UI ------------------------------\n")]
    [SerializeField] GameObject keyUI;
    [SerializeField] TMP_Text addedKeysText;

    [Header("------------------------------ OTHER ------------------------------\n")]
    public GameObject playerDamageScreen;
    public Camera playerCam;
    float timeScaleOrig;
    public float gravity;
    public GameObject loadingScreen;
    public TMP_Text loadingText;
    public Image loadingBar;
    public GameObject loadingReady;
    public GameObject saveMessage;
    public GameObject deleteWarning;
    public GameObject quitWarning;
    public GameObject blockScreen;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (!isTitleScreen)
        {
            playerSpawnPOS = GameObject.FindWithTag("Respawn");

            player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<RigidPlayer>();
            playerInventory = player.GetComponent<Inventory>();
            playerCam = FindObjectOfType<Camera>();
            damageDone = PlayerPrefs.GetInt("DamageDone", 0);
            timeScaleOrig = Time.timeScale;
            if (!Application.isEditor)
                AudioManager.instance.PlayMusic(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        loadingScreen.SetActive(false);
        blockScreen.SetActive(false);
    }

    void Update()
    {
        if (!isTitleScreen)
            PauseMenu();

        if (!isDialoguePlaying && dialogueQueue.Count > 0)
        {
            string nextDialogue = dialogueQueue.Dequeue();
            StartCoroutine(TypeText(nextDialogue));
        }
        
        if ((menuActive != null || isTitleScreen) && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && EventSystem.current.currentSelectedGameObject == null)
        {

            EventSystem.current.SetSelectedGameObject(Selectable.allSelectablesArray[0].gameObject);
            blockScreen.SetActive(true);
        }
        else if ((menuActive != null || isTitleScreen) && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetMouseButtonDown(0)))
        {
            blockScreen.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void TriggerDialogue(string message)
    {
        dialogueQueue.Enqueue(message);
    }

    IEnumerator TypeText(string line)
    {
        isDialoguePlaying = true;

        dialoguePanel.SetActive(true);
        dialogueText.text = ""; //Clears Text

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(0.75F / charactersPerSecond);
        }

        yield return new WaitForSecondsRealtime(2F);
        dialogueText.text = ""; //Clears Text
        dialoguePanel.SetActive(false);

        isDialoguePlaying = false;
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

    public void UpdateKeyUI(int keyCount)
    {
        keyUI.SetActive(keyCount > 0);
        addedKeysText.text = keyCount.ToString("00");
    }

    public void UpdateHPBar(int hpMissing, int hpTotal)
    {

        // if (hpMissing < 0) //Should Prevent UI from going Neg
        // {
        //     playerHPMissing.text = hpMissing.ToString("00");
        // }

        playerHPMissing.text = hpMissing.ToString("00");
        playerHPTotal.text = hpTotal.ToString("00");
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
            if (texture != null)
            {
                weaponIcon.texture = texture;
            }
        }
    }

    public void UpdateAmmoUI(GunStatsSO newWeapon)
    {

        ammoSizeText.text = newWeapon.ammoCount.ToString("00");
        ammoCountText.text = newWeapon.magAmmoCount.ToString("00");

        if (newWeapon.magAmmoCount == 0) //Should only call UI when mag is empty.
        {
            reloadMessage.SetActive(true);
        }
        else
        {
            reloadMessage.SetActive(false);
        }
    }

    public void YouLose()
    {
        StatePaused();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void Instructions()
    {
        menuActive = instructionsPage;
        menuActive.SetActive(true);
    }
    public void Options()
    {
        menuActive = optionsPage;
        menuActive.SetActive(true);
    }

    public void BackToMain()
    {
        menuActive.SetActive(false);
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
    }
}


