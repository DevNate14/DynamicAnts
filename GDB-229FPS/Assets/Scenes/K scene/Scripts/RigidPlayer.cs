using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RigidPlayer : MonoBehaviour
{

    Vector3 PlayerMovmentInput;
    Vector2 PlayerMouseInput;

    private float xRot;

    [Header("Body parts")]
    [SerializeField] private LayerMask Floormask;
    [SerializeField] private Transform Feet;
    [SerializeField] Transform Eyes;
    [SerializeField] Rigidbody Player;

    [Header("Movement")]
    [SerializeField] private float MoveSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float SprintSpeed;
    [SerializeField] float Sensitivity;
    [SerializeField] float Jumpforce;



    // Update is called once per frame
    void Update()
    {
        PlayerMovmentInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

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
            if (Physics.CheckSphere(Feet.position, 0.1f, Floormask))
            {
                Player.AddForce(Vector3.up * Jumpforce, ForceMode.Impulse);
            }
        }
    }

    void Sprint()
    {
        if (Input.GetKeyDown("left shift"))
        {
            MoveSpeed = SprintSpeed;
        }
        if (Input.GetKeyUp("left shift"))
        {
            SprintSpeed = walkSpeed;
        }
    }

    void MovePlayerCamera()
    {
        xRot -= PlayerMouseInput.y * Sensitivity;

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        Eyes.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

}
