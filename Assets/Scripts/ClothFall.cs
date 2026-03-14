using UnityEngine;
 // Add this to access XR components

public class ClothFall : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabScript;

    void Start()
    {
        // Get the grab component and disable it at the start
        grabScript = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grabScript != null)
        {
            grabScript.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            FixedJoint joint = GetComponent<FixedJoint>();
            if (joint != null)
            {
                Destroy(joint);

                // Now that it fell, let the player pick it up!
                if (grabScript != null)
                {
                    grabScript.enabled = true;
                }
            }
        }
    }
}