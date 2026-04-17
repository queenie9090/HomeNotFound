using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using Unity.XR.CoreUtils;
using System.Collections;

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

    // We need to declare this here so the whole script can use it!
    private XROrigin originComponent;

    void Start()
    {
        // Get the component once at the start
        if (xrOrigin != null)
        {
            originComponent = xrOrigin.GetComponent<XROrigin>();
        }

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
        // 4. Seated Mode Logic
        int isSeated = PlayerPrefs.GetInt("SeatedMode", 0);

        if (originComponent != null)
        {
            if (isSeated == 1)
            {
                originComponent.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
                originComponent.CameraYOffset = 2.7f;
            }
            else
            {
                originComponent.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
                originComponent.CameraYOffset = 0f;
            }

            // Start the coroutine properly
            StartCoroutine(ForceHeightOffset(isSeated));
        }

        Debug.Log($"[Real-Time] Settings applied. Seated: {isSeated}");
    }
    // Moved OUTSIDE of ApplySettings
    IEnumerator ForceHeightOffset(int isSeated)
    {
        yield return new WaitForEndOfFrame();

        if (originComponent != null && originComponent.CameraFloorOffsetObject != null)
        {
            if (isSeated == 0)
                originComponent.CameraFloorOffsetObject.transform.localPosition = new Vector3(0, 1.2f, 0);
            else
                originComponent.CameraFloorOffsetObject.transform.localPosition = Vector3.zero;
        }
    }

    void LateUpdate()
    {
        // This is the "Brute Force" safety net
        int isSeated = PlayerPrefs.GetInt("SeatedMode", 0);

        if (originComponent != null && isSeated == 0)
        {
            if (originComponent.CameraFloorOffsetObject != null)
            {
                // We keep forcing it to 1.5f so the VR hardware can't reset it to 0
                originComponent.CameraFloorOffsetObject.transform.localPosition = new Vector3(0, 1.2f, 0);
            }
        }
    }


    void ApplyAudioSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        AudioListener.volume = savedVolume;
    }
}