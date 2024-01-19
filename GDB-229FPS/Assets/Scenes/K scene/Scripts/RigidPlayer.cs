using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;


public class RigidPlayer : MonoBehaviour 
{
    //inputs to move player and camera
    GameObject Maincamera;
    Vector3 PlayerMovmentInput;
    Vector2 PlayerMouseInput;
    //camera rotaion 
   // private float xRot;

    [Header("Body parts")]
    [SerializeField] Transform Feet;          
    [SerializeField] Rigidbody Player;
    [SerializeField] float Groundraylength;
   
    //movement variabels 
    [Header("Movement")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float SprintSpeed;
    [SerializeField]float temp ;

    [Header("Eye Sensitivity")]
    [SerializeField] float Sensitivity;

    [Header("Jump")]
    [SerializeField] float Jumpforce;
    [SerializeField] int jumpedtimes;
    [SerializeField] int jumpMax;
    [SerializeField] float superjumpe;
    bool Grounded;

    [Header("CROUCHING")]
    [SerializeField] float CrouchScale;
    [SerializeField] float CrounchSpeed;
    [SerializeField] float StandingScale;
    bool Crouching;

    [Header("Slope")]
    public float MaxslopeAngel;
    private RaycastHit slophit;

    [Header("stairs")]
    [SerializeField]GameObject stepup;
    [SerializeField]GameObject whatsinfront;
    [SerializeField] float stepHeight;
    [SerializeField] float smoothwalk;

    private void Start()
    {
        Maincamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    // Update is called once per frame
    void Update()
    {
        
        PlayerMovmentInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //ayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        // player height 
        StandingScale = transform.localScale.y;
        //STAIRS
        
        Debug.DrawRay(stepup.transform.position, stepup.transform.forward);
        //ground check
        Debug.DrawRay(Feet.position, transform.TransformDirection(Vector3.down * Groundraylength));
        
        RaycastHit Hit;
        if (Physics.Raycast(Feet.position, Vector3.down,out Hit, Groundraylength))
        {
            Grounded = true;

            jumpedtimes = 0;
           
        }
        

        MovePlayer();
        //MovePlayerCamera();
        
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovmentInput) * MoveSpeed;
        Player.velocity = new Vector3(MoveVector.x, Player.velocity.y, MoveVector.z);
       
        Sprint();
        Crouch();
        Stairs();
        Jump();
        
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out GroundCheck, Groundraylength))
        //{
        //    groundedPlayer = true;
        //    //sets the players up and down velocity to 0 

        //    //rests jump to 0 once player lands
        //    jumpedtimes = 0;
        //}
        //else
        //{
        //    groundedPlayer = false;
        //}
        // walk  up slope
        if (OnSlop())
        {
            Player.AddForce(GetslopeMove() * SprintSpeed * 20f,ForceMode.Force);
            if(Player.velocity.y >0)
                Player.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
       
       
    }

    void Jump()
    {
        if (Input.GetKeyDown("space") && jumpedtimes < jumpMax)
        {
            jumpedtimes++;
            Player.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
           
            if (jumpedtimes == jumpMax)
            {
                Player.AddForce(Vector3.down, ForceMode.Impulse);

            }
           
            

        }

    }
    void Sprint()
    {
        

        if (Input.GetKeyDown("left shift"))
        {
            Debug.Log("IM RUNNING");
            temp = SprintSpeed;
            MoveSpeed = temp;
            
        }
        if (Input.GetKeyUp("left shift"))
        {
            Debug.Log("IM WALKING");
            temp = walkSpeed;
            MoveSpeed = temp;

        }
    }

    void Crouch()
    {
        float temp;
        temp = StandingScale;
        //check if grouded check button if false
        if (Grounded && Input.GetButtonDown("Crouch") && Crouching==false)
        {
            Crouching = true;
            //change local y scale
            // how do i keep the camera from moving 
           transform.localScale = new Vector3(transform.localScale.x, StandingScale * CrouchScale,transform.localScale.z);
           transform.position=new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            Debug.Log("Im crounching");
            //decrement speed
            

        }//check if grouded check button if true
        else if (Grounded && Input.GetButtonDown("Crouch") && Crouching == true)
        {
            Crouching = false;
            //set height back to normal 
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            transform.localScale = new Vector3(transform.localScale.x, StandingScale / CrouchScale, transform.localScale.z);
            //give player back speed 
            Debug.Log("Im not crounching");
        }
        if (Grounded && Input.GetButtonDown("Crouch") ==true && Input.GetKeyDown("space"))
        {
            Vector3 vector3 = (Maincamera.transform.forward * (superjumpe * 100)) + (Maincamera.transform.up * (superjumpe/40)) + Vector3.zero;
            Player.AddRelativeForce(vector3, ForceMode.Impulse);

        }
    }

    private bool OnSlop()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out slophit, StandingScale * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slophit.normal);
            return angle < MaxslopeAngel && angle != 0;
        }// proirty
         // 
         return false;
    }

    private Vector3 GetslopeMove()
    {
        return Vector3.ProjectOnPlane(PlayerMovmentInput, slophit.normal).normalized;
    }

    void Stairs()
    {

        RaycastHit low;


        if (Physics.Raycast(whatsinfront.transform.position,transform.TransformDirection(Vector3.forward), out low, 0.1f)) 
        {
            if (!Physics.Raycast(stepup.transform.position, transform.TransformDirection(Vector3.forward), out _, 0.2f))
            {

                Player.position -= new Vector3(0f, -stepHeight, 0f);
                
            }

        }
        
        // if i do two ray cast one on th bottom another by the knes 
        // then chck fo an objct in front
        // check if room at top then pass in no roomisa all move alng 
        //look into this  low priorty 
        // back up plan invisible slope over stairs 
    }

    void Pullup()
    {
        //look for edge  if edge pull up 
    }

    //void MovePlayerCamera()
    //{
    //    xRot -= PlayerMouseInput.y * Sensitivity;

    //    transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
    //    Eyes.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    //}

  
}
