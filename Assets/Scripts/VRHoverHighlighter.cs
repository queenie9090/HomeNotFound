using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRHoverHighlighter : MonoBehaviour
{
    private Outline outline;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable; 

    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null) outline.enabled = false;

        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
    }

    void OnEnable()
    {
        if (interactable != null)
        {
            // Using the updated event names
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
        }
    }

    void OnDisable()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (outline != null) outline.enabled = true;
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (outline != null) outline.enabled = false;
    }
}