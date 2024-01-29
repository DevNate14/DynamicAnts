using UnityEngine;

public class Counter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("times", PlayerPrefs.GetInt("times", -1) + 1);
            this.enabled = false;
        }
    }
}
