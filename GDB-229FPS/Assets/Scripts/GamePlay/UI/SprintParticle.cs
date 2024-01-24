using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintParticle : MonoBehaviour
{
    public RigidPlayer playerController;
    [SerializeField] ParticleSystem sprintingParticle;


    void Start()
    {
        sprintingParticle.Stop();
    }

     void Update()
    {
        PlayParticles();
    }


    public void PlayParticles()
    {
        if (playerController != null)
        {
            bool isSprinting = playerController.IsSprinting(); 
            Debug.Log("Is Sprinting " + isSprinting);

            if (isSprinting) sprintingParticle.Play();
            else sprintingParticle.Stop();
        }
    }
}
