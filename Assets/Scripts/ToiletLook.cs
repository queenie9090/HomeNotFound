using UnityEngine;

public class ToiletLock : MonoBehaviour
{
    public Collider blockerCollider; 

    void Update()
    {
        if (NpcBoneDetector.npcDistracted)
        {
            blockerCollider.enabled = false; 
        }
        else
        {
            blockerCollider.enabled = true; 
        }
    }
}