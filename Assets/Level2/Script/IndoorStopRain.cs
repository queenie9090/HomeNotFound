using UnityEngine;

public class IndoorStopRain : MonoBehaviour
{
    public ParticleSystem rainSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rainSystem.Stop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rainSystem.Play();
        }
    }
}