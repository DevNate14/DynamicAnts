using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Created this so the project doesnt scream when trying to load, feel free to change whatever
public class Item : MonoBehaviour
{
    public int Length { get; set; }
    private string name;
    // Start is called before the first frame update
    void Start()
    {
        this.Length = 0;
        this.name = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName(string _name)
    {
        name = _name;
    }
    public string GetName()
    {
        return name;
    }
    public void AddUse()
    {

    }
}
