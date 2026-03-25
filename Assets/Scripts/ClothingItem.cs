using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ClothingItem : MonoBehaviour
{
    public AudioClip wearSound;

    [Header("Dialogue")]
    public string wearMessage = "Finally, something to wear.";

    private bool isWorn = false;
    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnEnteredSocket);
    }

    private void OnEnteredSocket(SelectEnterEventArgs args)
    {
        // Check if the interactor is a Socket
        if (args.interactorObject is XRSocketInteractor socket && !isWorn)
        {
            isWorn = true;

            // 1. LOCK THE SOCKET: Stop the socket from ever taking another item
            socket.socketActive = false;

            // 2. Play sound at player's head
            if (wearSound != null)
            {
                AudioSource.PlayClipAtPoint(wearSound, Camera.main.transform.position);
            }

            if (DialogueManager.Instance != null)
                DialogueManager.Instance.ShowDialogue(wearMessage);

            // 3. Update Journal
            if (JournalManager.Instance != null)
            {
                JournalManager.Instance.CompleteTask(0);
            }
            MissionManager.Instance.MarkClothComplete();
            // 4. Deactivate the cloth
            Invoke("DeactivateCloth", 0.05f);
        }
    }

    void DeactivateCloth()
    {
        this.gameObject.SetActive(false);
    }
}