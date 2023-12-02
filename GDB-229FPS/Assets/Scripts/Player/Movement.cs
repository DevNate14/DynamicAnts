using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour, IDamageable, IImpluse
{
    [SerializeField] CharacterController controller;
    [SerializeField] Inventory inventory;
    [SerializeField] float speed, jumpHeight, sprintMod, gravity;
    [SerializeField] int jumpMax, HP, shootrate, originalHP;
    [SerializeField] GameObject bullet, gunAttachPoint;
    private Vector3 playerVelocity;
    private bool grounded;
    private Vector3 move;
    private int jumpCount;
    bool shooting, ctrl, Space; //last two track thekeys since unity in editor does not like two key events running simultaneously
    public bool hasLongJump; // will make a better item inventory asap
    // Start is called before the first frame update
    void Start()
    {
        originalHP = HP;
        respawn();
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
        if (Input.GetButtonDown("Crouch")) {
            ctrl = true;
        }
        if (Input.GetButtonUp("Crouch")) {
            ctrl = false;
        }
        if (Input.GetButtonDown("Jump")) {
            Space = true;
        }
        if (Input.GetButtonUp("Jump")) {
            Space = false;
        }
        if (ctrl) {
            LongJump();
        }
        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * speed);
        if (Input.GetButtonDown("Shoot") && !shooting) {
            StartCoroutine(shoot());
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVelocity.y = jumpHeight;
            ++jumpCount;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void respawn(){
        HP = originalHP;
        UpdatePlayerUI();
        controller.enabled = false;
        transform.position = GameManager.instance.playerSpawn.transform.position;
        controller.enabled = true;
    }
    void WeaponSwap() { // this will have to be updated for each new weapon added, limits us to twelve items until we get a ui element up for weapons and a working scrollwheel implented
        //if (Input.GetButtonDown("Weapon Slot 1")) { Inventory.SelectedWeapon = 1; }
        //if (Input.GetButtonDown("Weapon Slot 2")) { Inventory.SelectedWeapon = 2; }
        //if (Input.GetButtonDown("Weapon Slot 3")) { Inventory.SelectedWeapon = 3; }
        //if (Input.GetButtonDown("Weapon Slot 4")) { Inventory.SelectedWeapon = 4; }
        //if (Input.GetButtonDown("Weapon Slot 5")) { Inventory.SelectedWeapon = 5; }

        //if (Inventory.GetWeapon()!= null) {
        //    SetWeapon(Inventory.GetWeapon());
        //}
    }
    void SetWeapon(Weapon weapon) {
        Instantiate(weapon, gunAttachPoint.transform);
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
        if (hasLongJump) {
            if (Space && transform.position.y < 2.5) {
                AddImpluse(25);
            }
        }
    }
    IEnumerator shoot()
    {

        shooting = true;
        Instantiate(bullet,gunAttachPoint.transform.position,  Camera.main.transform.rotation);
        yield return new WaitForSeconds(shootrate);

        shooting = false;
    }

    public void Damage(int amount) {
        HP -= amount;
        UpdatePlayerUI();
        if (HP <= 0) { 
            // loss state
            GameManager.instance.YouLose();
        }
    }

    public void AddImpluse(int magnitude) {
        Vector3 impulse = transform.forward * (magnitude * 2) +(transform.up * magnitude);
        controller.SimpleMove(impulse);
    }
    public float GetGravity() {
        return gravity;
    }
    
    public void UpdatePlayerUI(){
        GameManager.instance.playerHPBar.fillAmount = (float)HP / originalHP;
    }
}
