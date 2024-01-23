using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintParticle : MonoBehaviour
{
    public Controller Controller;
    [SerializeField] ParticleSystem sprintingParticle;
    
    void Update()
    {

        if (Controller != null)
        {
            bool isSprinting = Controller.Sprint();
        
            if (!sprintingParticle.isPlaying)
            {
                sprintingParticle.Play();
            }

            else 
            {
                sprintingParticle.Stop();
            }
        }
    }
}
