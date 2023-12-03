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
    [SerializeField] CharacterController controller;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpMax, HP;
    [SerializeField] float gravity;
    [SerializeField] float sprintMod;
    [SerializeField] GameObject GunAttachPoint;
    public bool HasLongJump; // will make a better item inventory asap
    // Start is called before the first frame update
    void Start()
    {

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
        move = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(move * Time.deltaTime * speed);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVelocity.y = jumpHeight;
            ++jumpCount;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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
