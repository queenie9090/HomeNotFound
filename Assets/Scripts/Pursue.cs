using UnityEngine;

public class Pursue : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float rotationSpeed = 4f;
    public float keepDistance = 2f;

    public void DoPursue()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        float distance = direction.magnitude;

        // Rotate toward player
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(direction),
            rotationSpeed * Time.deltaTime
        );

        if (distance > keepDistance)
        {
            float slowRadius = 5f;
            float currentSpeed = speed;

            // Arrival
            if (distance < slowRadius)
            {
                currentSpeed = speed * (distance / slowRadius);
            }

            transform.position += direction.normalized * currentSpeed * Time.deltaTime;
        }
    }
}
