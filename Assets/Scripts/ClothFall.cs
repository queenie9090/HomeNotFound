using UnityEngine;

public class ClothFall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Only fall if the thing hitting us has the "Stone" tag
        if (collision.gameObject.CompareTag("Stone"))
        {
            // This finds the joint holding the cloth up and destroys it
            FixedJoint joint = GetComponent<FixedJoint>();
            if (joint != null)
            {
                Destroy(joint);
            }
        }
    }
}