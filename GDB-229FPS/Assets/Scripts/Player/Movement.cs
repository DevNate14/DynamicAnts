using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IDamageable, IImpluse
{
    public Inventory Inventory;
    private Vector3 playerVelocity;
    private bool grounded;
    private Vector3 move;
    private int jumpCount;
    private float currSpeed;
    [SerializeField] CharacterController controller;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpMax, HP;
    [SerializeField] float gravity;
    [SerializeField] float sprintMod;
    [SerializeField] GameObject GunAttachPoint;
    [SerializeField] float maxSpeed;
    int HPOrig;

    public bool isCrouched; //Bool is public for GameManager to check
    public bool HasLongJump; // will make a better item inventory asap
    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        isCrouched = false;
        RespawnPlayer();
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
        if (Input.GetButtonDown("Crouch") && grounded)
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
            //this is for whether we decide to allow the player to jump more than once. without this line, the player wont gain velocity when pressing again after a long fall
            playerVelocity.y = 0;
            playerVelocity.y = jumpHeight;
            ++jumpCount;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

     public void RespawnPlayer()
    {
        HP = HPOrig;
        UpdatePlayerUI();

        controller.enabled = false;
        //transform.position = GameManager.instance.playerSpawnPOS.transform.position;
        //NEED TO ADD PLAYER SPAWN POS IN UNITY!!!!
        controller.enabled = true;
    }

      public void UpdatePlayerUI()
    {
         GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
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
    void WeaponSwap() { // this will have to be updated for each new weapon added, limits us to twelve items until we get a ui element up for weapons and a working scrollwheel implented
        if (Input.GetButtonDown("Weapon Slot 1")) { Inventory.SelectedWeapon = 1; }
        if (Input.GetButtonDown("Weapon Slot 2")) { Inventory.SelectedWeapon = 2; }
        if (Input.GetButtonDown("Weapon Slot 3")) { Inventory.SelectedWeapon = 3; }
        if (Input.GetButtonDown("Weapon Slot 4")) { Inventory.SelectedWeapon = 4; }
        if (Input.GetButtonDown("Weapon Slot 5")) { Inventory.SelectedWeapon = 5; }

        if (Inventory.GetWeapon()!= null) {
            SetWeapon(Inventory.GetWeapon());
        }
    }
    void SetWeapon(Weapon weapon) {
        Instantiate(weapon, GunAttachPoint.transform);
        //weapon.transform.SetParent();
    }
    void Sprint()
    {

        if (Input.GetButtonDown("Sprint"))
            speed *= sprintMod;
        else if (Input.GetButtonUp("Sprint"))
            speed /= sprintMod;
    }
    void LongJump() {
        if (Input.GetButtonDown("Sprint") && Input.GetButtonDown("Jump") && jumpCount < jumpMax) {
            //Was going to respect longjump's original intention, decided to work on something else at the moment
            AddImpluse(new Vector3(-20,-10,0));
        }
    }

    public void Damage(int amount) {
        HP -= amount;
        if (HP <= 0) { 
            // loss state
        }
    }

    public void Heal(int amount) {
        HP += amount;
    }

    public void AddImpluse(Vector3 magnitude) {
        playerVelocity -= magnitude;
    }
    public float GetGravity() {
        return gravity;
    }
    public void UpgradeItem(Item item) {
        Inventory.UpgradeItem(Inventory.FindItem(item));
    }
}
