using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ClothFall : MonoBehaviour
{
    private XRGrabInteractable grabScript;

    void Start()
    {
        grabScript = GetComponent<XRGrabInteractable>();
        if (grabScript != null) grabScript.enabled = false;

        // NEW: Tell Unity physics that NO FORCE is strong enough to break this joint.
        // Only our script can Destroy it now.
        FixedJoint joint = GetComponent<FixedJoint>();
        if (joint != null)
        {
            joint.breakForce = Mathf.Infinity;
            joint.breakTorque = Mathf.Infinity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Now, this is the ONLY way the cloth can ever fall:
        if (collision.gameObject.CompareTag("Stone"))
        {
            FixedJoint joint = GetComponent<FixedJoint>();
            if (joint != null)
            {
                Destroy(joint);

                if (grabScript != null)
                {
                    grabScript.enabled = true;
                }

                Debug.Log("Hit by Stone! Falling now.");
            }
        }
        else
        {
            Debug.Log("Hit by " + collision.gameObject.name + ", but it's not a Stone, so I'm staying up!");
        }
    }
}