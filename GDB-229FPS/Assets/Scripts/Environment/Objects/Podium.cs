using System.Collections;
using UnityEngine;

public class Podium : MonoBehaviour
{
    //black //green
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip clip;
    [SerializeField] Color color1, color2;
    [SerializeField] Renderer[] bulbs;
    [SerializeField] GameObject[] Keys;
    [SerializeField] GameObject beacon;
    [SerializeField] GameObject portalBack;
    [SerializeField] GameObject portalWin;
    [SerializeField] string[] winMessages;
    [SerializeField] string[] partialMessages;
    [SerializeField] string[] failMessages;
    Inventory keys;
    int keyCurr = 0;
    bool activated = false;
    bool isChecking = false;
    private void Start()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //Inventory bag = player.GetComponent<Inventory>();
        keys = GameManager.instance.playerInventory;
        for (int i = 0; i < 3; i++)
        {
            bulbs[i].material.color = color1;
            Keys[i].SetActive(false);
        }
    }
    private void Update()
    {
        if(activated && !isChecking && keyCurr < 3)
        {
            StartCoroutine(CheckKeys());
        }
        if(keyCurr >= 3)
        {
            switch (GameManager.instance.playerInventory.KeyCount())
            {
                case 0:
                    foreach (string message in failMessages)
                    { GameManager.instance.TriggerDialogue(message); }
                    portalBack.SetActive(true);
                    break;
                case 3:
                    foreach (string message in winMessages)
                    { GameManager.instance.TriggerDialogue(message); }
                    portalWin.SetActive(true);
                    break;
                default:
                    foreach (string message in partialMessages)
                    { GameManager.instance.TriggerDialogue(message); }
                    portalBack.SetActive(true);
                    break;
            }
            beacon.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;
        }
    }
    public IEnumerator CheckKeys()
    {
        isChecking = true;
        if (keys.CheckKeyById(keyCurr + 1))
        {
            Keys[keyCurr].SetActive(true);
            bulbs[keyCurr].material.color = color2;
            aud.PlayOneShot(clip);
        }
        yield return new WaitForSeconds(1);
        keyCurr++;
        isChecking = false;
    }
}
