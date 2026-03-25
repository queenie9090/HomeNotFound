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
        // ONLY do this if it's the bread and the dog hasn't already snatched it
        if (other.CompareTag("Bread") && !isRunningAway)
        {
            Debug.Log("[STATE]: Bread Snatched - Triggering Feedback");

            snatchedBread = other.gameObject;
            isRunningAway = true;

            // 1. PLAY THE SNATCH SOUND
            if (snatchSource != null)
            {
                snatchSource.Play();
                Debug.Log("[AUDIO]: Playing Crunch Sound at " + snatchSource.volume + " volume.");
            }
            else
            {
                Debug.LogError("[AUDIO]: SnatchSource is MISSING in the Inspector!");
            }

            // 2. TRIGGER HAPTICS (Vibration)
            // We get the grab component to find which hand is holding it
            var interactable = snatchedBread.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (interactable != null && interactable.isSelected)
            {
                // This sends the vibration to the specific controller holding the bread
                interactable.interactorsSelecting[0].transform.GetComponent<ActionBasedController>().SendHapticImpulse(0.7f, 0.2f);
                Debug.Log("[FEEDBACK]: Haptic Impulse Sent to Player");
            }

            // 3. LOGIC: Disable grab and physics
            if (interactable != null) interactable.enabled = false;

            Collider breadCollider = other.GetComponent<Collider>();
            if (breadCollider != null) breadCollider.enabled = false;

            Rigidbody breadRb = other.GetComponent<Rigidbody>();
            if (breadRb != null) breadRb.isKinematic = true;


            // 3. Play the Sequence
            if (DialogueManager.Instance != null)
                StartCoroutine(PlaySnatchSequence());

            if (JournalManager.Instance != null)
            {
                JournalManager.Instance.CompleteTask(1); // Mark in the book
            }

            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.MarkDogComplete(); // Update the level exit logic
            }

            // 4. ATTACH TO MOUTH
            snatchedBread.transform.SetParent(mouthPoint);
            foreach (Transform child in snatchedBread.transform)
            {
                child.SetParent(null);
            }
            snatchedBread.transform.localPosition = Vector3.zero;

            Debug.Log("[STATE]: Running to Escape Point");
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