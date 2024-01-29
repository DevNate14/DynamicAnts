using UnityEngine;

public class SprintParticle : MonoBehaviour
{
    public RigidPlayer playerController;
    [SerializeField] ParticleSystem sprintingParticle;
    [SerializeField] float minRunSpeed = 5;
    private Rigidbody playerRigidbody;
    void Start()
    {
        sprintingParticle.Stop();
        playerRigidbody = playerController.GetComponent<Rigidbody>();
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
            float playerVel = playerRigidbody.velocity.magnitude;
            isSprinting = playerVel > minRunSpeed && isSprinting;
            if (isSprinting) sprintingParticle.Play();
            else sprintingParticle.Stop();
        }
    }
}
