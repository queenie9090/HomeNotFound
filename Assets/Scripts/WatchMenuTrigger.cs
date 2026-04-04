using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HybridWatchTrigger : MonoBehaviour
{
    public PauseManager pauseManager;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        // This listens for the Ray "Select" (Trigger Pull)
        interactable.selectEntered.AddListener(OnRayClick);
    }

    // Triggered by the Ray
    private void OnRayClick(SelectEnterEventArgs args)
    {
        Debug.Log("<color=cyan>[Watch]</color> Clicked by Ray!");
        pauseManager.TogglePause();
    }

    // Triggered by Physical Touch (Finger Poke)
    private void OnTriggerEnter(Collider other)
    {
       
            Debug.Log("<color=green>[Watch]</color> Touched by Finger!");
            pauseManager.TogglePause();
        
    }
}