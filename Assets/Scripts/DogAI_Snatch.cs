using UnityEngine;


public class DogAI_Snatch : MonoBehaviour
{
    public GameObject dogObject;   // The Dog NPC (Inactive at start)
    public Transform mouthPoint;   // Empty object in Dog's mouth
    public Transform escapePoint;  // Where the dog runs to (City Gate)
    public Animator dogAnim;

    public float runSpeed = 4f;
    public AudioSource barkSource;

    private bool isRunningAway = false;
    private GameObject snatchedBread;

    public void AppearAndSnatch()
    {
        if (!dogObject.activeSelf)
        {
            dogObject.SetActive(true);

            // Trigger the bark exactly when he appears
            if (barkSource != null)
            {
                barkSource.Play();
            }
            if (dogAnim != null) dogAnim.SetBool("IsRunning", true);
            Debug.Log("Dog appeared and barked!");
        }
    }

    void Update()
    {
        if (dogObject.activeSelf)
        {
            if (!isRunningAway)
            {
                // 1. Find the bread in the scene
                GameObject bread = GameObject.FindWithTag("Bread");

                if (bread != null)
                {
                    // 2. Aim for the BREAD position
                    Vector3 targetPos = bread.transform.position;
                    targetPos.y = dogObject.transform.position.y; // Keep dog paws on the ground

                    // 3. Move and Look at the Bread
                    dogObject.transform.position = Vector3.MoveTowards(dogObject.transform.position, targetPos, runSpeed * Time.deltaTime);
                    dogObject.transform.LookAt(targetPos);
                }
            }
            else
            {
                // Move toward the streets (The Escape Phase)
                dogObject.transform.position = Vector3.MoveTowards(dogObject.transform.position, escapePoint.position, runSpeed * Time.deltaTime);
                dogObject.transform.LookAt(escapePoint.position);

                // --- NEW: Check if we reached the end ---
                float distToGate = Vector3.Distance(dogObject.transform.position, escapePoint.position);

                if (distToGate < 0.2f)
                {
                    // Option A: Just turn the dog off
                    dogObject.SetActive(false);

                    // Option B: If you want the bread to disappear too
                    if (snatchedBread != null) snatchedBread.SetActive(false);

                    if (Vector3.Distance(dogObject.transform.position, escapePoint.position) < 0.2f)
                    {
                        if (dogAnim != null) dogAnim.SetBool("IsRunning", false);
                        Invoke("TurnOffDog", 1.5f); // Wait a bit so we see the idle/sit
                    }
                    Debug.Log("Dog reached the city and vanished.");
                }
            }
        }
    }

    // 2. The physical snatch happens when the Dog touches the Bread
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bread") && !isRunningAway)
        {
            snatchedBread = other.gameObject;
            isRunningAway = true;

            // Force player to drop bread
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = snatchedBread.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (grab != null) grab.enabled = false;

            // 1. STOP THE SNAG: Disable the bread's collider immediately
            // This prevents the bread from "hooking" the trash bin as the dog runs
            Collider breadCollider = other.GetComponent<Collider>();
            if (breadCollider != null) breadCollider.enabled = false;

            // 2. STOP THE PHYSICS: Make it Kinematic so it doesn't fall or bounce
            Rigidbody breadRb = other.GetComponent<Rigidbody>();
            if (breadRb != null) breadRb.isKinematic = true;

            // Chomp! Attach bread to mouth
            snatchedBread.transform.SetParent(mouthPoint);
            snatchedBread.transform.localPosition = Vector3.zero;
            snatchedBread.GetComponent<Rigidbody>().isKinematic = true;

            isRunningAway = true; // Change direction to the City Gate
            Debug.Log("Dog snatched and is running away!");
        }
    }
}