using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort; 

public class PlayerAcceptPunch : MonoBehaviour, ITunnelingVignetteProvider
{
    public AudioSource audioSource;
    public AudioClip punchDialogueClip;

    [Header("Hurt Settings")]
    [Tooltip("Drag the GameObject holding your TunnelingVignetteController here")]
    public TunnelingVignetteController vignetteController;
    public int PunchCounter = 0;

    [Header("Hurt Visuals")]
    [Tooltip("Configure your custom damage colors and sizes here!")]
    public VignetteParameters hurtParameters = new VignetteParameters()
    {
        apertureSize = 0.6f,
        featheringEffect = 0.3f,
        easeInTime = 0.1f, 
        easeOutTime = 0.5f, 
        vignetteColor = Color.red,
        vignetteColorBlend = new Color(1f, 0f, 0f, 0.5f)
    };

    public VignetteParameters vignetteParameters => hurtParameters;

    public void ReactToPunch()
    {
        PunchCounter++;

        if (PunchCounter > 3) return;

        if (audioSource != null && punchDialogueClip != null)
        {
            audioSource.PlayOneShot(punchDialogueClip);
        }

        if (vignetteController != null)
        {
            vignetteController.gameObject.SetActive(true);

            if (PunchCounter < 3)
            {
                vignetteController.BeginTunnelingVignette(this);

                CancelInvoke("HideHurtVision");

                Invoke("HideHurtVision", 1.0f);
            }
            else if (PunchCounter == 3)
            {
                CancelInvoke("HideHurtVision");
                vignetteController.BeginTunnelingVignette(this);

                Debug.Log("<color=red>Player knocked out! Hurt vision locked ON.</color>");
            }
        }
    }

    private void HideHurtVision()
    {
        if (vignetteController != null)
        {
            vignetteController.EndTunnelingVignette(this);
        }
    }
}