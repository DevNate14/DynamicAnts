using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImpluse
{
    // add to an enemy if you want them to be able to be thrown around by the players actions or possibly environmental effects, call this on the player or an enemy if you want this to happen.
    // example for this one will be the shotgun object, it might look complex but there will be comments explaining it.
    void AddImpluse(Vector3 _impulse, float resolveTime);
}
