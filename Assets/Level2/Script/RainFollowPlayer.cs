using UnityEngine;

public class RainFollowPlayer : MonoBehaviour
{
    [Header("References")]
    public Transform player;       
    public ParticleSystem rainSystem;

    [Header("Settings")]
    public float heightOffset = 10f;

    void Start()
    {
        if (rainSystem == null)
        {
            rainSystem = GetComponent<ParticleSystem>();
        }

        var main = rainSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    void LateUpdate()
    {
        if (player == null || rainSystem == null) return;

        // Follow player position
        Vector3 newPosition = player.position;
        newPosition.y += heightOffset;

        rainSystem.transform.position = newPosition;
    }
}