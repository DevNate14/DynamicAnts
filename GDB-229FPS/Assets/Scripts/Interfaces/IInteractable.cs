using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    // put this on an environmental object if you want the player to innteract with it, could do a lot more with this but we should stay consistent to the project. will add this to player so you all dont have to worry about it, just need to implement any interactions 
    // you want on your created object.
    bool Interact();
}
