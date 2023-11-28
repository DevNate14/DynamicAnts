using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockMin;
    [SerializeField] int lockMax;
    [SerializeField] bool invert;

    float xRot;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invert)
            xRot += mouseY;
        else
            xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, lockMin, lockMax);

        transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
