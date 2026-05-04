using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MatchingPuzzleManager : MonoBehaviour
{
    public static MatchingPuzzleManager Instance { get; private set; }

    [SerializeField] private List<MatchingSocket> sockets = new List<MatchingSocket>();

    [Tooltip("Invoked once when all sockets are correctly filled.")]
    public UnityEvent OnPuzzleCompleted;

    [Tooltip("Invoked if the puzzle becomes incomplete after being completed (e.g. mineral removed).")]
    public UnityEvent OnPuzzleUncompleted;

    public bool IsPuzzleComplete { get; private set; } = false;

    private void Awake()
    {
        // Simple singleton so sockets can find the manager easily
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // Optional: auto-find sockets in the scene if list is empty
    private void Start()
    {
        if (sockets.Count == 0)
        {
            sockets.AddRange(FindObjectsByType<MatchingSocket>(FindObjectsSortMode.None));
        }
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
            Debug.Log("Puzzle completed!");
            OnPuzzleCompleted?.Invoke();
        }
        else if (!allCorrect && IsPuzzleComplete)
        {
            IsPuzzleComplete = false;
            Debug.Log("Puzzle no longer complete.");
            OnPuzzleUncompleted?.Invoke();
        }
    }
}