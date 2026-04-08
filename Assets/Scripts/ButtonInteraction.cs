using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public Animator doorAnimator;
    public ParticleSystem particle;

    void Update()
    {
        // Press keyboard E (for testing in editor)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Activate();
        }
    }

    void Activate()
    {
        // Open door
        doorAnimator.SetBool("isOpen", true);

        // Start particle system
        particle.Play();
    }
}