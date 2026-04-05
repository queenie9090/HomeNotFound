using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HybridWatchTrigger : MonoBehaviour
{
    public PauseManager pauseManager;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    private float lastToggleTime;
    public float cooldown = 0.5f; // Prevents accidental double-clicks

    void Awake()
    {
        //interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        //interactable.selectEntered.AddListener(OnRayClick);
    }

    private void OnRayClick(SelectEnterEventArgs args)
    {
        ExecuteToggle();
    }

    // This handles the physical "Poke"
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("<color=red>[Watch]</color> Something hit me: " + other.name);
        // Check if the thing touching us is a Poke Interactor (Finger)
        if (other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRPokeInteractor>() != null)
        {
            ExecuteToggle();
        }
    }

    private void ExecuteToggle()
    {
        // Increase cooldown to 1.0 second to prevent the "Instant Close" bug
        if (Time.unscaledTime - lastToggleTime > 1.0f)
        {
            lastToggleTime = Time.unscaledTime;

            if (pauseManager != null)
            {
                pauseManager.TogglePause();
                // This log will tell us if it actually stayed open
                Debug.Log("<color=green>[Watch]</color> Toggle Successful! Time: " + Time.time);
            }
        }
        else
        {
            Debug.Log("<color=orange>[Watch]</color> Blocked accidental double-toggle.");
        }
    }
}