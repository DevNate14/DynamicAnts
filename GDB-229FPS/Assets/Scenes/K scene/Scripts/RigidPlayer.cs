using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;


public class RigidPlayer : MonoBehaviour
{

    Vector3 PlayerMovmentInput;
    Vector2 PlayerMouseInput;
    //camera rotaion 
    private float xRot;

    [Header("Body parts")]
   // [SerializeField] private LayerMask Floormask;
    [SerializeField] private Transform Feet;
    [SerializeField] Transform Eyes;
    [SerializeField] Rigidbody Player;
    [SerializeField] float Groundraylength;
   

    [Header("Movement")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float SprintSpeed;
    private float temp ;

    [Header("Eye Sensitivity")]
    [SerializeField] float Sensitivity;

    [Header("Jump")]
    [SerializeField] float Jumpforce;
    private bool groundedPlayer;
    bool Grounded;

    [Header("CROUCHING")]
    [SerializeField] float CrouchScale;
    [SerializeField] float CrounchSpeed;
    [SerializeField] float StandingScale;
    bool Crouching;

    [Header("Slope")]
    public float MaxslopeAngel;
    private RaycastHit slophit;
    
    // Update is called once per frame
    void Update()
    {
        
        PlayerMovmentInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //ground check
        Debug.DrawRay(Feet.position, transform.TransformDirection(Vector3.down * Groundraylength),Color.red);
        Grounded = Physics.Raycast(transform.position, Vector3.down, Groundraylength);
        // player height 
        StandingScale = transform.localScale.y;
       
        MovePlayer();
        MovePlayerCamera();
        
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovmentInput) * walkSpeed;
        Player.velocity = new Vector3(MoveVector.x, Player.velocity.y, MoveVector.z);
        Sprint();
        Crouch();

        if(OnSlop())
        {
            Player.AddForce(GetslopeMove() * SprintSpeed * 20f,ForceMode.Force);
            if(Player.velocity.y >0)
                Player.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (Input.GetKeyDown("space"))
        {
            // if (Physics.CheckSphere(Feet.position, 0.1f, Floormask))
           

            Player.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
          
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
        if (Input.GetKeyDown("right shift"))
        {
            
            //Crouching = true;
            //change local y scale
            // how do i keep the camera from moving 
           transform.localScale = new Vector3(transform.localScale.x, CrouchScale-= temp,transform.localScale.z);

           Debug.Log("Im crounching");
            //decrement speed
            

        }//check if grouded check button if true
        if (Input.GetKeyUp("right shift"))
        {
            // Crouching = false;
            //set height back to normal 
            transform.localScale = new Vector3(transform.localScale.x, temp+=CrouchScale, transform.localScale.z);
            //give player back speed 
            Debug.Log("Im not crounching");
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
        //look into this  low priorty 
        // back up plan invisible slope over stairs 
    }

    void Pullup()
    {
        //look for edge  if edge pull up 
    }

    void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        Eyes.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

  
}
