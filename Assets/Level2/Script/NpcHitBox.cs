using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NpcHitbox : MonoBehaviour
{
    public BegPunchPlayer mainScript;
    public float punchVelocityThreshold = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            float distToLeft = Vector3.Distance(transform.position, mainScript.leftHand.position);
            float distToRight = Vector3.Distance(transform.position, mainScript.rightHand.position);

            float impactSpeed = (distToLeft < distToRight) ? mainScript.currentLeftSpeed : mainScript.currentRightSpeed;

            Debug.Log($"<color=orange>Punch Detected!</color> Speed: {impactSpeed:F2} | Needed: {punchVelocityThreshold}");

            if (impactSpeed > punchVelocityThreshold)
            {
                ActionBasedController controller = other.GetComponentInParent<ActionBasedController>();
                if (mainScript != null)
                {
                    mainScript.OnPunch(controller);
                }
            }
        }
    }
}