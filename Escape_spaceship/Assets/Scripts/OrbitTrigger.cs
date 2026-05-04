using UnityEngine;

public class OrbitTrigger : MonoBehaviour
{
    public GameObject trophy;
    public AudioSource successSound;
    public ParticleSystem successParticles;

    public OrbitSystem orbitSystem;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (!other.CompareTag("Player") && !other.CompareTag("MainCamera"))
            return;

        // Show trophy
        if (trophy != null)
            trophy.SetActive(true);

        // Play sound
        if (successSound != null)
            successSound.Play();

        // Play particles
        if (successParticles != null)
            successParticles.Play();

        // Stop orbit motion
        if (orbitSystem != null)
            orbitSystem.triggered = true;
    }
}