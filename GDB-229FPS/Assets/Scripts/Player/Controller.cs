using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour, IDamageable, IImpluse
{
    private Vector3 playerVelocity, impulse;
    private bool grounded;
    private Vector3 move;
    private int jumpCount;
    private float currSpeed, impulseResolve;
    private int HPOrig;
    public int ammoSize;
    public int ammoCount;
    private GameObject anchor;
    [SerializeField] CharacterController controller;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpMax, HP;
    [SerializeField] float gravity;
    [SerializeField] float sprintMod;
    [SerializeField] public GameObject gunModel, muzzlePoint;
    [SerializeField] float maxSpeed;
    [SerializeField] Animator animator;
    

    public bool isCrouched; //Bool is public for GameManager to check
    public bool HasLongJump; // will make a better item inventory asap
    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        isCrouched = false;
        RespawnPlayer();
        //anchor = 
    }

    // Update is called once per frame
    void Update()
    {
        
        grounded = controller.isGrounded;
        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            jumpCount = 0;
        }
        Sprint();
        if (HasLongJump) {
            LongJump();
        }
        // Can be crouch or changed to sneak, using grounded assuming we allow the player to jump multiple times in the future
        if (Input.GetButtonDown("Crouch") && jumpCount == 0)
        {
            ToggleCrouch();
        }
        if (!grounded)
        {
        }
        else
        {
        }
        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * speed);
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            animator.Play("Jump");
            //this is for whether we decide to allow the player to jump more than once. without this line, the player wont gain velocity when pressing again after a long fall
            playerVelocity.y = 0;
            playerVelocity.y = jumpHeight;
            ++jumpCount;
        }
        // Moved certain functions to allow a smooth animation
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move((playerVelocity + impulse) * Time.deltaTime);
        impulse = Vector3.Lerp(impulse, Vector3.zero, Time.deltaTime * impulseResolve);
    }

     public void RespawnPlayer()
    {
        HP = HPOrig;
        UpdatePlayerUI();

        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawnPOS.transform.position;
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
    }

    public void UpdateAmmoUI(GunStatsSO newWeapon)
    {
        
        GameManager.instance.ammoSizeText.text = ammoSize.ToString("00");
        GameManager.instance.ammoCountText.text = ammoCount.ToString("00");
        
    }

    //Made toggle for ease of use
    void ToggleCrouch()
    {
        if (!isCrouched)
            speed /= sprintMod;
        else
            speed *= sprintMod;
        isCrouched = !isCrouched;
    }
    void Sprint()
    {
        if (!isCrouched)
            return;
        if (Input.GetButtonDown("Sprint"))
            speed *= sprintMod;
        else if (Input.GetButtonUp("Sprint"))
            speed /= sprintMod;
    }
    void LongJump() {
        if (isCrouched && Input.GetButtonDown("Jump") && jumpCount < jumpMax) {
            //Was going to respect longjump's original intention, decided to work on something else at the moment
            impulse = transform.forward * 10 + transform.up * 5;
            impulseResolve = 1;
            // we need a check to zero out impulse after landing from a jump since the lerp likes to drag the player along after landing, cant be a grounded check since it would never allow the player 
            // to long jump, need a new bool like "longJumped" thattracks the sequence of events will add if noone else gets to it later
        }
    }

    public void Damage(int amount) {
        HP -= amount;
        
        if (HP <= 0) { 
            GameManager.instance.YouLose();
        }

        UpdatePlayerUI();
        StartCoroutine(PlayerFlashDamage());
    }

    public void Heal(int amount) {
        HP += amount;
    }

    public void AddImpluse(Vector3 _impulse, float resolveTime) {
        impulse = _impulse;
        impulseResolve = resolveTime;
    }
    public float GetGravity() {
        return gravity;
    }
    IEnumerator Animate()
    {
        if (animator.GetBool("Jump"))
        {
            animator.Play("Player_Jump");
            yield return new WaitForSeconds(1);
            animator.SetBool("Jump", false);
            animator.SetBool("InAir", true);
        }
        if (playerVelocity.y > 0)
            animator.Play("Player_MidAir");
        else if (playerVelocity.y < 0)
            animator.Play("Player_Fall");
    }
}
