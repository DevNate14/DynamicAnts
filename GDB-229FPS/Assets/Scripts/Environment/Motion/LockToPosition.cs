using UnityEngine;

public class LockToPosition : MonoBehaviour
{
    Vector3 Pos;
    void Start()
    {
        Pos = transform.position;
    }
    void Update()
    {
        transform.position = Pos;
    }
}
