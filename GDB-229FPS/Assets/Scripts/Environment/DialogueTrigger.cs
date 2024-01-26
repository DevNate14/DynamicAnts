using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // [SerializeField] string text;
    [SerializeField] string[] dialogueLines;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            foreach (string line in dialogueLines)
            {
                GameManager.instance.TriggerDialogue(line);
            }
            Destroy(gameObject);
        }
    }
}
