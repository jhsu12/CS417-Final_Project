using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MatchingPuzzleManager : MonoBehaviour
{
    [SerializeField] private List<MatchingSocket> sockets = new List<MatchingSocket>();

    [Header("Completion Effects")]
    [Tooltip("Particle system to play once when the puzzle is completed. Can be placed anywhere in the scene.")]
    [SerializeField] private ParticleSystem completionParticles;

    [Tooltip("Sound effect to play once when the puzzle is completed.")]
    [SerializeField] private AudioClip completionSound;

    [SerializeField] private AudioSource audioSource;

    [Header("Events")]
    [Tooltip("Invoked once when all sockets are correctly filled.")]
    public UnityEvent OnPuzzleCompleted;

    [Tooltip("Invoked if the puzzle becomes incomplete after being completed (e.g. mineral removed).")]
    public UnityEvent OnPuzzleUncompleted;

    public bool IsPuzzleComplete { get; private set; } = false;

    private bool hasPlayedCompletionEffects = false;

    private void Awake()
    {
        // Auto-create an AudioSource if none was assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
            }
        }
    }

    private void Start()
    {
    }

    public void CheckPuzzleState()
    {
        bool allCorrect = true;
        foreach (var socket in sockets)
        {
            if (socket == null || !socket.IsCorrectlyFilled)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect && !IsPuzzleComplete)
        {
            IsPuzzleComplete = true;
            Debug.Log("Matching Puzzle completed!");
            PlayCompletionEffects();
            OnPuzzleCompleted?.Invoke();
        }
    }

    private void PlayCompletionEffects()
    {
        if (hasPlayedCompletionEffects) return;
        hasPlayedCompletionEffects = true;

        if (completionParticles != null)
        {
            completionParticles.Play();
        }

        if (completionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(completionSound);
        }
    }
}