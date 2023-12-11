using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedContact : MonoBehaviour
{
    [SerializeField] int MinDamage;
    [SerializeField] int MaxDamage;
    [SerializeField] float DamageFraction;
    private float Speed;
    private float LastPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other) //okay so if you move too fast this gets called after you land
    {
        if (other.isTrigger)
            return;
        LastPosition = other.transform.position.y;
        StartCoroutine(Damage(other));
    }

    IEnumerator Damage(Collider other)
    {
        //yield return new WaitForEndOfFrame();
        //yield return new WaitForFixedUpdate();
        yield return 0;
        Debug.Log(LastPosition);
        Debug.Log(other.transform.position.y);
        Speed = (LastPosition - other.transform.position.y) / Time.deltaTime;
        Debug.Log(Speed);
        IDamageable dmg = other.GetComponent<IDamageable>();
        if (dmg != null)
        {
            dmg.Damage((int)Mathf.Clamp(Speed * DamageFraction, MinDamage, MaxDamage));
        }
    }
}
