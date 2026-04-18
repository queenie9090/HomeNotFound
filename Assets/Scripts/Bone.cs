using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoneTask : MonoBehaviour
{
    private XRGrabInteractable grab;

    private bool hasCompleted = false;

    void Start()
    {
        grab = GetComponent<XRGrabInteractable>();

        if (grab != null)
        {
            grab.selectEntered.AddListener(OnGrab);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (hasCompleted) return;

        hasCompleted = true;

        Debug.Log("Bone grabbed!");

        if (JournalInLv3.Instance != null)
        {
            JournalInLv3.Instance.CompleteTask(0); // Task 1
        }
    }
}