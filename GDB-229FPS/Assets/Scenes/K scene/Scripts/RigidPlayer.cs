using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    bool Crouching;
    
    
    // Update is called once per frame
    void Update()
    {
        
        PlayerMovmentInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //ground check
       
        Debug.DrawRay(Feet.position, transform.TransformDirection(Vector3.down * Groundraylength),Color.red);
        Grounded = Physics.Raycast(transform.position, Vector3.down, Groundraylength);
       
        MovePlayer();
        MovePlayerCamera();
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovmentInput) * MoveSpeed;
        Player.velocity = new Vector3(MoveVector.x, Player.velocity.y, MoveVector.z);
        Sprint();

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

            temp = SprintSpeed;
            MoveSpeed = temp;
            
        }
        if (Input.GetKeyUp("left shift"))
        {
          
            temp = walkSpeed;
            MoveSpeed = temp;
        }
    }

    void Crouch()
    {
        //check if grouded check button if false
        if (groundedPlayer && Input.GetButtonDown("Crouch") && Crouching == false)
        {
            Crouching = true;
            //change local y scale
            
            //decrement speed
            

        }//check if grouded check button if true
        else if (groundedPlayer && Input.GetButtonDown("Crouch") && Crouching == true)
        {
            Crouching = false;
            //set height back to normal 
            
            //give player back speed 
            
        }
    }

    void Slop()
    {
        // proirty  
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
