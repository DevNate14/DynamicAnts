using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrowableObject : MonoBehaviour, IImpluse
{
    [SerializeField] Rigidbody rb;
    public void AddImpluse(Vector3 _impulse, float resolveTime)
    {
        rb.AddForce(_impulse * 10);
    }
}
