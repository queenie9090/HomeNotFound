using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class PlayerAcceptPunch : MonoBehaviour, ITunnelingVignetteProvider
{
    public AudioSource audioSource;
    public AudioClip punchDialogueClip;

    [Header("Hurt Settings")]
    [Tooltip("Drag ONLY your duplicated HurtVignette here!")]
    public TunnelingVignetteController redVignetteController;
    public int PunchCounter = 0;

    [Header("Hurt Visuals")]
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
        if (PunchCounter >= 3) return;

        PunchCounter++;

        if (audioSource != null && punchDialogueClip != null)
        {
            audioSource.PlayOneShot(punchDialogueClip);
        }

        if (redVignetteController != null)
        {
            redVignetteController.gameObject.SetActive(true);

            if (PunchCounter < 3)
            {
                redVignetteController.BeginTunnelingVignette(this);
                CancelInvoke("HideHurtVision");
                Invoke("HideHurtVision", 1.0f);
            }
            else if (PunchCounter == 3)
            {
                CancelInvoke("HideHurtVision");

                redVignetteController.BeginTunnelingVignette(this);

                Debug.Log("<color=red>Player knocked out! Red vision locked ON permanently.</color>");
            }
        }
    }

    private void HideHurtVision()
    {
        if (redVignetteController != null)
        {
            redVignetteController.EndTunnelingVignette(this);
        }
    }
}