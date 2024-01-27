using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Podium : MonoBehaviour
{
    
    //black //green
    [SerializeField] Color color1, color2;
    [SerializeField] Renderer[] bulbs;
    [SerializeField] GameObject[] Keeys;
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Inventory bag = player.GetComponent<Inventory>();
        CheckKeys(bag);
    }

    public bool CheckKeys(Inventory keys)
    {
        Keeys[0].SetActive(keys.CheckKeyById(1));
        if(keys.CheckKeyById(1) ) 
        {
            bulbs[0].material.color = color2;
            
        }
        else
        {
            bulbs[0].material.color = color1;
        }
        Keeys[1].SetActive(keys.CheckKeyById(2));
        if (keys.CheckKeyById(2))
        {
            bulbs[1].material.color = color2;
        }
        else
        {
            bulbs[1].material.color = color1;
        }
        Keeys[2].SetActive(keys.CheckKeyById(3));
        if (keys.CheckKeyById(3))
        {
            bulbs[2].material.color = color2;
        }
        else
        {
            bulbs[2].material.color = color1;
        }

        if (keys.CheckKeys())
        {
            return true;

        }
        else 
        {
            return false;
        }

    }
}
