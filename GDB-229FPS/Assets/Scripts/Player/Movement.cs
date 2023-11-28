using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 playerVelocity;
    private bool grounded;
    [SerializeField] CharacterController controller;
    [SerializeField] float speed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;
        if (grounded && playerVelocity.y < 0)
            playerVelocity.y = 0;


        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * speed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        if (Input.GetButtonDown("Jump") && grounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
