using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;



public class RigidPlayer : MonoBehaviour, IDamageable, IPersist, IImpluse
{
    [SerializeField] public AudioSource aud;
    //inputs to move player and camera
    [SerializeField] GameObject Maincamera;
    [SerializeField] float InteractRange;
    Vector3 PlayerMovmentInput;
    Vector2 PlayerMouseInput;
    public Vector3 playerSpawnPos;
    //camera rotaion 
    // private float xRot;

    [Header("Body parts")]
    [SerializeField] Transform Feet;
    [SerializeField] Rigidbody Player;
    [SerializeField] float Groundraylength;
    [Header("Weapons")]
    [SerializeField] public GameObject gunModel, muzzlePoint;
    [SerializeField] Animator GunAnim;
    [SerializeField] float GunAnimSpeed;
    [Header("Health")]
    [SerializeField] public int HPOrig;
    [SerializeField] int HP;
    [SerializeField] public int damageDone;
    //movement variabels 
    [Header("Movement")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float SprintSpeed;
    [SerializeField] float temp;

    public bool isSprinting = false;

    [Header("Eye Sensitivity")]
    [SerializeField] float Sensitivity;

    [Header("Jump")]
    [SerializeField] float Jumpforce;
    [SerializeField] int jumpedtimes;
    [SerializeField] int jumpMax;
    [SerializeField] float superjumpe;
    bool Grounded;
    Vector3 LongJump;
    [SerializeField] float LongJumpTime;

    [Header("CROUCHING")]
    [SerializeField] float CrouchScale;
    [SerializeField] float CrounchSpeed;
    [SerializeField] float StandingScale;
    bool Crouching;

    [Header("Slope")]
    public float MaxslopeAngel;
    private RaycastHit slophit;
    bool OnSlope;

    [Header("stairs")]
    [SerializeField] GameObject stepup;
    [SerializeField] GameObject whatsinfront;
    [SerializeField] float stepHeight;

    [Header("audio")]
    [Range(0, 1)] [SerializeField] float footstepsVol;
    [SerializeField] AudioClip[] footstepsSFX;
    [SerializeField] AudioClip[] jumpsSFX;
    [SerializeField] AudioClip[] landSFX;
    [SerializeField] AudioClip[] hurtSFX;
    bool isPlayingFootsteps;
    bool wasFalling;

    // [SerializeField] float smoothwalk;
    // Update is called once per frame
    void Start()
    {
        //player height
        StandingScale = transform.localScale.y;
        AddToPersistenceManager();
        HPOrig = HP;
        LoadState();
        SpawnPlayer();
    }

    void Update()
    {

        PlayerMovmentInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMovmentInput = PlayerMovmentInput.normalized; //Prevents Faster movement - moving Diagonally
        //ayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //STAIRS

        Debug.DrawRay(stepup.transform.position, stepup.transform.forward);
        Debug.DrawRay(stepup.transform.position, stepup.transform.forward + stepup.transform.right);
        Debug.DrawRay(stepup.transform.position, stepup.transform.forward + -stepup.transform.right);

        Debug.DrawRay(whatsinfront.transform.position, stepup.transform.forward, Color.green);
       
        //ground check
        Debug.DrawRay(Feet.position, transform.TransformDirection(Vector3.down * Groundraylength));
        RaycastHit[] hits = Physics.RaycastAll(Feet.position, Vector3.down, Groundraylength);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && hit.transform != this && !Grounded)
            {
                if (!hit.collider.isTrigger)
                {
                    Grounded = true;
                    jumpedtimes = 0;
                }
            }
        }

        if (wasFalling && Grounded)
        {
            aud.PlayOneShot(landSFX[Random.Range(0, landSFX.Length)], 1);
            wasFalling = false;
        }

        MovePlayer();
        BounceGun();
        //MovePlayerCamera();
        pickup();

    }

    private void MovePlayer()
    {
        Sprint();
        Crouch();
        Stairs();
        Jump();
        Vector3 MoveVector = transform.TransformDirection(PlayerMovmentInput) * MoveSpeed;
        
        if(MoveVector.magnitude > 0.1f && !isPlayingFootsteps)
        {
            StartCoroutine(PlayFootsteps());
        }

        Player.velocity = MoveVector + new Vector3(LongJump.x, Player.velocity.y, LongJump.z);

        if (OnSlope = OnSlop())
        {
            Grounded = true;
            jumpedtimes = 0;
            //    Debug.Log("Slope");
            //    //Player.AddForce(GetslopeMove() * SprintSpeed * 20f, ForceMode.Force);
            //    Player.velocity = GetslopeMove();
            //    if (Player.velocity.y > 0.3f)
            //        Player.AddForce(Vector3.down * 80f, ForceMode.Force);
        }


    }

    void Jump()
    {
        LongJump = Vector3.Lerp(LongJump, Vector3.zero, LongJumpTime * Time.deltaTime);
        if (Input.GetKeyDown("space") && jumpedtimes < jumpMax)
        {
            if (Crouching)
            {
                Debug.Log("Super Jump");
                LongJump += (Maincamera.transform.forward * 12.5f) / 4 * superjumpe;
                //Vector3 vector3 = transform.forward * superjumpe;

                Debug.Log(LongJump);
                Player.AddForce(LongJump, ForceMode.Impulse);
                Crouching = false;
                //set height back to normal 
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
                transform.localScale = new Vector3(transform.localScale.x, StandingScale, transform.localScale.z);
                //give player back speed 
                Debug.Log("Im not crounching");
            }
            //Player.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
            Player.velocity += Vector3.up * Jumpforce;
            Grounded = false;
            wasFalling = true;
            jumpedtimes++;
            aud.PlayOneShot(jumpsSFX[Random.Range(0, jumpsSFX.Length)]);
            //if (jumpedtimes >= jumpMax)
            //{
            //    Player.AddForce(Vector3.down, ForceMode.Impulse);

            //}
        }

    }

    void Sprint()
    {

        if (Input.GetKeyDown("left shift"))
        {
            temp = SprintSpeed;
            MoveSpeed = temp;
            isSprinting = true;
            Debug.Log("IM RUNNING");
        }

        if (Input.GetKeyUp("left shift"))
        {
            temp = walkSpeed;
            MoveSpeed = temp;
            isSprinting = false;
            Debug.Log("IM WALKING");
        }
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }

    void Crouch()
    {
        float temp;
        temp = StandingScale;
        //check if grouded check button if false
        if (Grounded && Input.GetButtonDown("Crouch") && Crouching == false)
        {
            Crouching = true;
            //change local y scale
            // how do i keep the camera from moving 
            transform.localScale = new Vector3(transform.localScale.x, CrouchScale, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            Debug.Log("Im crounching");
            //decrement speed


        }//check if grouded check button if true
        else if (Grounded && Input.GetButtonDown("Crouch") && Crouching == true)
        {
            Crouching = false;
            //set height back to normal 
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, StandingScale, transform.localScale.z);
            //give player back speed 
            Debug.Log("Im not crounching");
        }

    }

    private bool OnSlop()
    {

        if (Physics.Raycast(whatsinfront.transform.position, Vector3.down, out slophit, 0.3f))
        {
            //Debug.Log("Slope");
            if (slophit.collider.isTrigger)
                return false;
            float angle = Vector3.Angle(Vector3.up, slophit.normal);
            //Debug.Log(angle < MaxslopeAngel && angle != 0);
            return angle < MaxslopeAngel && angle != 0;
        }// proirty
         // 
        return false;
    }

    //private Vector3 GetslopeMove()
    //{
    //    return Vector3.ProjectOnPlane(new Vector3(Player.velocity.x, 0, Player.velocity.z), slophit.normal).normalized;
    //}

    void Stairs()
    {

        RaycastHit low;
        RaycastHit[] high = new RaycastHit[3];


        if (Physics.Raycast(whatsinfront.transform.position, transform.forward, out low, 0.1f) && !OnSlope)
        {
            if (low.collider.isTrigger)
                return;
            int num = 0;
            for (int i = 0; i < 3; i++)
            {
                if (!Physics.Raycast(stepup.transform.position, transform.forward, out high[i], 0.2f))
                    num++;
                else if (high[i].collider.isTrigger)
                    num++;
            }

            Debug.Log(num);
            if (num == 3)
            {
                Player.position += new Vector3(0f, stepHeight, 0f);
            }
            else if (num <= 1)
            {
                Player.AddForce(-transform.forward, ForceMode.Impulse);

            }

        }

        // if i do two ray cast one on th bottom another by the knes 
        // then chck fo an objct in front
        // check if room at top then pass in no roomisa all move alng 
        //look into this  low priorty 
        // back up plan invisible slope over stairs 
    }

    IEnumerator PlayFootsteps()
    {
        isPlayingFootsteps = true;

        float currentVol = Grounded ? (Crouching ? footstepsVol / 2 : footstepsVol) : 0;
        
        aud.PlayOneShot(footstepsSFX[Random.Range(0, footstepsSFX.Length)], currentVol);

        if(isSprinting)
        {
            yield return new WaitForSeconds(0.2f);
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
        }

        isPlayingFootsteps = false;
    }

    void pickup()
    {
        Debug.DrawRay(Maincamera.transform.position, Maincamera.transform.forward * InteractRange, Color.blue);
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit[] iteam = Physics.RaycastAll(Maincamera.transform.position, Maincamera.transform.forward, InteractRange);
            foreach (RaycastHit raycastHit in iteam)
            {
                if (raycastHit.collider != null)
                {
                    if (raycastHit.collider != this)
                    {
                        IInteractable thing = raycastHit.transform.GetComponent<IInteractable>();

                        if (thing != null)
                        {
                            thing.Interact();
                            return;
                        }
                    }
                }
            }

        }
        // how does this work  find a object with the e  button then give interacbele
    }

    //void Pullup()
    //{
    //    //look for edge  if edge pull up 
    //    //This can be done easily with the same logic you have for stairs just a raycast that is slightly further out and checking more area
    //    //only caveat is we have to set a rule to only make things in the level that keep that spacing rule in mind or else we can get some weird bugs like 
    //    //one I saw in someones project where a flawed ray check let you "vault" through a wall
    //}

    public void UpdatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        GameManager.instance.UpdateHPBar(HP, HPOrig);
    }

    public void RespawnPlayer()
    {
        HP = HPOrig;
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        UpdatePlayerUI();
        //impulse = Vector3.zero;
        //impulseResolve = 0; // I think this fits but rework by player person may have altered the logic to make this line pointless or even dangerous
        transform.position = playerSpawnPos;

    }

    IEnumerator PlayerFlashDamage()
    {
        GameManager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScreen.SetActive(false);
    }

    public void ShowTotalDamage() // same as other needed damage rework
    {
        GameManager.instance.DisplayDamageDone(damageDone);
    }

    public void Damage(int amount)
    {
        HP -= amount;
        aud.PlayOneShot(hurtSFX[Random.Range(0, hurtSFX.Length)]);
        if (HP <= 0)
        {
            GameManager.instance.YouLose();
        }
        UpdatePlayerUI();
        StartCoroutine(PlayerFlashDamage());
    }
    //public void AddImpluse(Vector3 _impulse, float resolveTime)
    //{
    //    impulse = _impulse;
    //    impulseResolve = resolveTime;
    //}
    public void Heal(int amount)
    {
        HP += amount;
        UpdatePlayerUI();
    }

    public void AddToPersistenceManager()
    {
        PersistenceManager.instance.AddToManager(this);
    }

    public void SaveState()
    {
        PlayerPrefs.SetInt("PlayerCurrHP", HP);

        //PlayerPrefs.SetFloat("SpawnPosX", playerSpawnPos.x);
        //PlayerPrefs.SetFloat("SpawnPosY", playerSpawnPos.y);
        //PlayerPrefs.SetFloat("SpawnPosZ", playerSpawnPos.z);
    }

    public void LoadState()
    {
        HP = PlayerPrefs.GetInt("PlayerCurrHP", HPOrig);

        playerSpawnPos = new Vector3(PlayerPrefs.GetFloat("SpawnPosX"), PlayerPrefs.GetFloat("SpawnPosY"), PlayerPrefs.GetFloat("SpawnPosZ"));
    }

    //void MovePlayerCamera()
    //{
    //    xRot -= PlayerMouseInput.y * Sensitivity;

    //    transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
    //    Eyes.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    //}

    public void ApplyBuff(int type) {
        switch (type)
        {
            case 1:
                Heal(HPOrig - HP);
                break;
        }
    }

    public void AddImpluse(Vector3 _impulse, float resolveTime)
    {
        LongJump += _impulse;
        Player.velocity += new Vector3(0, _impulse.y, 0);
    }

    private void BounceGun()
    {
        GunAnim.SetFloat("Movement", Mathf.Lerp(GunAnim.GetFloat("Movement"), (MoveSpeed - walkSpeed) / 5, Time.deltaTime * GunAnimSpeed));
    }
}
