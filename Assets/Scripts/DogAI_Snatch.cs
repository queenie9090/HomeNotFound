using UnityEngine;
using UnityEngine.AI; // Required for NavMesh
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class DogAI_Snatch : MonoBehaviour
{
    public GameObject dogObject;
    public Transform mouthPoint;
    public Transform escapePoint;
    public Animator dogAnim;
    public NavMeshAgent agent; // Drag the NavMeshAgent component here

    public float runSpeed = 4f;
    public AudioSource barkSource;
    public AudioSource snatchSource;

    [Header("Custom Dialogue")]
    [TextArea(3, 10)]
    public string[] snatchDialogue = { "Hey! My bread!", "Nevermind... he looks hungrier than me." };

    private bool isRunningAway = false;
    private GameObject snatchedBread;

    public void AppearAndSnatch()
    {
        if (!dogObject.activeSelf)
        {
            dogObject.SetActive(true);
            Debug.Log("[STATE]: Dog Spawned - Seeking Bread");

            if (barkSource != null) barkSource.Play();
            if (dogAnim != null) dogAnim.SetBool("IsRunning", true);

            // Set initial speed for NavMesh
            if (agent != null) agent.speed = runSpeed;
        }
    }

    void Update()
    {
        if (dogObject.activeSelf && agent != null)
        {
            if (!isRunningAway)
            {
                GameObject bread = GameObject.FindWithTag("Bread");
                if (bread != null)
                {
                    // NavMesh takes care of moving and looking at the target
                    agent.SetDestination(bread.transform.position);
                }
            }
            else
            {
                // Seeking the escape point
                agent.SetDestination(escapePoint.position);

                // Check if reached destination using Agent logic
                if (!agent.pathPending && agent.remainingDistance < 0.2f)
                {
                    if (agent.speed > 0)
                    {
                        Debug.Log("[STATE]: Dog reached Gate - Vanishing soon");
                        agent.speed = 0; // Stop the agent
                        if (dogAnim != null) dogAnim.SetBool("IsRunning", false);
                        Invoke("TurnOffDog", 0.5f);
                    }
                }
            }
        }
    }

    void TurnOffDog()
    {
        dogObject.SetActive(false);
        if (snatchedBread != null) snatchedBread.SetActive(false);
        Debug.Log("[STATE]: Dog Inactive - Left Alley");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bread") && !isRunningAway)
        {
            snatchedBread = other.gameObject;
            isRunningAway = true;

            if (snatchSource != null) snatchSource.Play();

            var interactable = snatchedBread.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

            // 1. DISCONNECT FROM PLAYER FIRST
            if (interactable != null && interactable.isSelected)
            {
                var interactor = interactable.interactorsSelecting[0];
                var controller = interactor.transform.GetComponentInParent<ActionBasedController>();
                if (controller != null) controller.SendHapticImpulse(0.7f, 0.2f);

                interactable.interactionManager.CancelInteractableSelection((UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable)interactable);
            }

            // 2. CHANGE LAYER IMMEDIATELY 
            // This is the "Magic Fix" for the MissingReference errors.
            // It makes the XR system stop looking at the bread instantly.
            snatchedBread.layer = LayerMask.NameToLayer("Ignore Raycast");

            // 3. ATTACH TO MOUTH (Do this before disabling components)
            snatchedBread.transform.SetParent(mouthPoint);
            snatchedBread.transform.localPosition = Vector3.zero;
            snatchedBread.transform.localRotation = Quaternion.identity;
            // 4. PHYSICS & COMPONENT CLEANUP
            if (snatchedBread != null)
            {
                // Use ?. (Null-conditional operator) to safely check if the component exists
                // and hasn't been destroyed yet by the Editor shutdown.

                if (interactable != null) interactable.enabled = false;

                var outline = snatchedBread.GetComponentInChildren<Outline>();
                if (outline != null) outline.enabled = false;

                var highlighter = snatchedBread.GetComponentInChildren<VRHoverHighlighter>();
                if (highlighter != null) highlighter.enabled = false;

                Rigidbody breadRb = snatchedBread.GetComponent<Rigidbody>();
                if (breadRb != null)
                {
                    breadRb.isKinematic = true;
                    breadRb.detectCollisions = false;
                }

                Collider breadCol = snatchedBread.GetComponent<Collider>();
                if (breadCol != null) breadCol.enabled = false;
            }

            // 5. EXTERNAL SYSTEMS
            if (DialogueManager.Instance != null) StartCoroutine(PlaySnatchSequence());
            if (JournalManager.Instance != null) JournalManager.Instance.CompleteTask(1);
            if (MissionManager.Instance != null) MissionManager.Instance.MarkDogComplete();

            Debug.Log("[STATE]: Bread Snatched and Reparented");
        }
    }

    IEnumerator PlaySnatchSequence()
    {
        // Line 1: Immediate reaction
        DialogueManager.Instance.ShowDialogue("Hey! My bread!");
        yield return new WaitForSeconds(2.5f); // Adjust this to make it faster/slower

        // Line 2: The realization
        DialogueManager.Instance.ShowDialogue("Nevermind... he looks hungrier than me.");
        yield return new WaitForSeconds(3.5f);
    }

}