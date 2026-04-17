using UnityEngine;

public class ToiletLock : MonoBehaviour
{
    public Collider blockerCollider; // drag your wall collider here

    void Update()
    {
        // Lock before task, unlock after task
        if (NpcBoneDetector.npcDistracted)
        {
            blockerCollider.enabled = false; // allow player in
        }
        else
        {
            blockerCollider.enabled = true; // block player
        }
    }
}