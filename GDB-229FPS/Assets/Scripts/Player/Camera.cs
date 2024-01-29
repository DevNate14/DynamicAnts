using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockMin;
    [SerializeField] int lockMax;
    [SerializeField] bool invert;
    float xRot;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sensitivity = PlayerPrefs.GetInt("MouseSensitivity", sensitivity);
        invert = PlayerPrefs.GetInt("InvertY", invert ? 1 : 0) == 1;
    }
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
