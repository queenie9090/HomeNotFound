using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StealMoney : MonoBehaviour
{
    public Level2Manager manager;
    public StoleDetector stole;
    public StealPunchPlayer spp;

    public bool peekingRemy = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip takeSound;

    void Update()
    {

    }

    // Called by XR Interaction
    public void OnRaySelect()
    {

        if (audioSource != null && takeSound != null)
            audioSource.PlayOneShot(takeSound);

        if(stole != null)
        stole.setActiveStole();

        manager.AddMoney(6);

        gameObject.SetActive(false);

        if(peekingRemy)
        {
            if(spp != null)
            {
                spp.SetNotActive();
            }
        }
    }
}