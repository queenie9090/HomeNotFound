using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using Unity.XR.CoreUtils;

public class MovementSettingApplier : MonoBehaviour
{
    [Header("Providers")]
    public LocomotionProvider teleportProvider;
    public LocomotionProvider continuousMoveProvider;
    public LocomotionProvider snapTurnProvider;
    public LocomotionProvider continuousTurnProvider;

    [Header("Comfort & Accessibility")]
    public GameObject tunnelingVignetteObject;
    public GameObject xrOrigin;

    void Start()
    {
        // Run everything once when the level loads
        ApplySettings();
        ApplyAudioSettings();
    }

    // This is the ONLY function you need to call from your UI buttons/toggles
    public void ApplySettings()
    {
        // 1. Movement Logic (Make sure these strings match your MainMenuManager!)
        int moveMode = PlayerPrefs.GetInt("MovementType", 0);
        if (teleportProvider != null) teleportProvider.enabled = (moveMode == 0);
        if (continuousMoveProvider != null) continuousMoveProvider.enabled = (moveMode == 1);

        // 2. Turning Logic
        int turnMode = PlayerPrefs.GetInt("TurningType", 0);
        if (snapTurnProvider != null) snapTurnProvider.enabled = (turnMode == 0);
        if (continuousTurnProvider != null) continuousTurnProvider.enabled = (turnMode == 1);

        // 3. Vignette Logic
        int vignetteOn = PlayerPrefs.GetInt("VignetteEnabled", 1);
        if (tunnelingVignetteObject != null)
        {
            tunnelingVignetteObject.SetActive(vignetteOn == 1);
        }

        // 4. Seated Mode Logic
        int isSeated = PlayerPrefs.GetInt("SeatedMode", 0);
        XROrigin origin = xrOrigin.GetComponent<XROrigin>();

        if (origin != null)
        {
            if (isSeated == 1)
            {
                origin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
                // Set this to a height that is clearly different from your standing height
                origin.CameraYOffset =2.5f;
            }
            else
            {
                origin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
                // When switching back to floor, the offset usually doesn't matter, 
                // but some setups prefer it at 0
                origin.CameraYOffset = 2.0f;
            }
        }

            Debug.Log($"[Real-Time] Settings: Move {moveMode}, Turn {turnMode}, Vig {vignetteOn}, Seated {isSeated}");
    }

    void ApplyAudioSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        AudioListener.volume = savedVolume;
    }
}