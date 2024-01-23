using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour, IDamageable, IImpluse, IPersist
{
    
    private Vector3 playerVelocity, impulse;
    private bool grounded;
    private Vector3 move;
    private int jumpCount;
    private float impulseResolve;
    public int HPOrig;
    public int ammoSize;
    public int ammoCount;
    public int damageDone;
    public float currSpeed;
    [SerializeField] public AudioSource aud;
    [SerializeField] CharacterController controller;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpMax, HP;
    [SerializeField] float gravity;
    [SerializeField] float sprintMod;
    [SerializeField] public GameObject gunModel, muzzlePoint;
    [SerializeField] float maxSpeed;
    [SerializeField] Animator animator;
    [SerializeField] AudioClip longJumpSound;
    
    public bool isCrouched; //Bool is public for GameManager to check
    public bool HasLongJump; // will make a better item inventory asap

    public Vector3 playerSpawnPos;

    // Start is called before the first frame update
    void Start()
    {
        AddToPersistenceManager();
        HPOrig = HP;
        isCrouched = false;
        LoadState();
        SpawnPlayer();
        UpdatePlayerUI(); //This should help....?
    }

    // Update is called once per frame
    void Update()
    {
        currSpeed = move.magnitude * speed;
        animator.SetFloat("Speed", currSpeed/maxSpeed);
        grounded = controller.isGrounded;
        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpCount = 0;
        }
        Sprint();
        // Can be crouch or changed to sneak, using grounded assuming we allow the player to jump multiple times in the future
        if (Input.GetButtonDown("Crouch") && jumpCount == 0)
        {
            ToggleCrouch();
        }
        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * currSpeed);
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            if (HasLongJump && isCrouched)
                LongJump();
            Jump();
        }
        // Moved certain functions to allow a smooth animation
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move((playerVelocity + impulse) * Time.deltaTime);
        impulse = Vector3.Lerp(impulse, Vector3.zero, Time.deltaTime * impulseResolve);
        if (impulse.z > 0.8f && impulse.y > 0.8f && grounded)
        {
            impulse = Vector3.zero;
            impulseResolve = 0;
        }
    }

    public void RespawnPlayer()
    {
        HP = HPOrig;
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        UpdatePlayerUI();
        impulse = Vector3.zero;
        impulseResolve = 0; // I think this fits but rework by player person may have altered the logic to make this line pointless or even dangerous
        controller.enabled = false;
        transform.position = playerSpawnPos;
        controller.enabled = true;
    }

    IEnumerator PlayerFlashDamage()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScreen.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        GameManager.instance.UpdateHPBar(HP,HPOrig);
    }

    // public void ShowTotalDamage() // same as other needed damage rework
    // {
    //     GameManager.instance.DisplayDamageDone(damageDone);
    // } //Need to Add Beta

    //Made toggle for ease of use
    void ToggleCrouch()
    {
        if (!isCrouched)
            speed /= sprintMod;
        else
            speed *= sprintMod;
        isCrouched = !isCrouched;
    }
    public bool Sprint()
    {
        if (isCrouched)
            return false;
            
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            return true;
        }

        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            return false;
        }
            return false; //No Sprint- By Default
    }
    void Jump()
    {
        playerVelocity.y = jumpHeight;
        ++jumpCount;
    }
    void LongJump() {
            //Was going to respect longjump's original intention, decided to work on something else at the moment
            ToggleCrouch();
            impulse = transform.forward * 10 + transform.up * 5;
            impulseResolve = 1;
            //aud.PlayOneShot(longJumpSound);
            // we need a check to zero out impulse after landing from a jump since the lerp likes to drag the player along after landing, cant be a grounded check since it would never allow the player 
            // to long jump, need a new bool like "longJumped" thattracks the sequence of events will add if noone else gets to it later
    }

    public void Damage(int amount)
    {
        HP -= amount;        
        if (HP <= 0) { 
            GameManager.instance.YouLose();
        }
        UpdatePlayerUI();
        StartCoroutine(PlayerFlashDamage());
    }

    public void Heal(int amount) {
        HP += amount;
        UpdatePlayerUI();
    }

    public void AddImpluse(Vector3 _impulse, float resolveTime) {
        impulse = _impulse;
        impulseResolve = resolveTime;
    }
    public float GetGravity() {
        return gravity;
    }
    public void ApplyBuff(int type)
    {
        switch (type)
        {
            default:
                //HasLongJump = true;
                //break;
            //case 1:
            //    ++jumpMax;
            //    break;
            //case 2:
            //    speed += 2;
            //    maxSpeed += 4;
            //    break;
            case 1:
                Heal(HPOrig - HP);
                //UpdatePlayerUI();
                break;
        }
    }

    public void AddToPersistenceManager()
    {
        PersistenceManager.instance.AddToManager(this);
    }
    public void SaveState()
    {
        PlayerPrefs.SetInt("PlayerCurrHP", HP);

        PlayerPrefs.SetFloat("SpawnPosX", playerSpawnPos.x);
        PlayerPrefs.SetFloat("SpawnPosY", playerSpawnPos.y);
        PlayerPrefs.SetFloat("SpawnPosZ", playerSpawnPos.z);
    }
    public void LoadState()
    {
        HP = PlayerPrefs.GetInt("PlayerCurrHP", HPOrig);

        playerSpawnPos = new Vector3(PlayerPrefs.GetFloat("SpawnPosX", GameManager.instance.playerSpawnPOS.transform.position.x), PlayerPrefs.GetFloat("SpawnPosY", GameManager.instance.playerSpawnPOS.transform.position.y), PlayerPrefs.GetFloat("SpawnPosZ", GameManager.instance.playerSpawnPOS.transform.position.z));
    }

}
