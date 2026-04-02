using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NpcHitbox : MonoBehaviour
{
    public BegPunchPlayer mainScript;
    public float punchVelocityThreshold = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing hitting the head is a hand
        // Make sure your VR Hands have a Tag "Player" or "Hand"
        if (other.CompareTag("Hand") || other.CompareTag("Player"))
        {
            // Get the velocity of the hand
            // If the hand has a Rigidbody, we use that. Otherwise, we can 
            // use a simple velocity check from a script on the hand.
            Rigidbody rb = other.GetComponent<Rigidbody>();
            float speed = (rb != null) ? rb.linearVelocity.magnitude : 3.0f; // Default to 3 if no RB

            if (speed > punchVelocityThreshold)
            {
                mainScript.OnPunch(null); // Trigger the flee logic
            }
        }
    }
}