using UnityEngine;

public class Wandering : MonoBehaviour
{
    public float speed = 1.5f;
    public float rotationSpeed = 4f;

    float wanderRadius = 5f;
    float wanderTimer = 3f;
    float timer;
    Vector3 wanderTarget;

    public void DoWander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            float randomX = Random.Range(-wanderRadius, wanderRadius);
            float randomZ = Random.Range(-wanderRadius, wanderRadius);

            wanderTarget = new Vector3(
                transform.position.x + randomX,
                transform.position.y,
                transform.position.z + randomZ
            );

            timer = 0;
        }

        Vector3 direction = wanderTarget - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime
            );

            transform.position += direction.normalized * speed * Time.deltaTime;
        }
    }
}
