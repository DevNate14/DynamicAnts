using UnityEngine;

public class KeyUnlock : MonoBehaviour
{
    [SerializeField] int KeyIndex;
    [SerializeField] Door Door;
    [SerializeField] bool AutoOpen;
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || !other.CompareTag("Player"))
            return;
        if (GameManager.instance.playerInventory.CheckKeyById(KeyIndex))
        {
            //Debug.Log("PLayewer has key");
            Door.Locked = false;
            if (AutoOpen)
            {
               //Debug.Log("Opening Door");
                Door.Open = true;
            }
        }
        else
        {
            //Debug.Log("Something didnt work");
        }
    }
}
