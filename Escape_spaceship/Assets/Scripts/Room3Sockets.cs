using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Room3Sockets : MonoBehaviour
{
    [SerializeField] private string acceptedTag;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Room3PuzzleManager puzzleManager;

    [SerializeField] private ParticleSystem correctParticles;

    private XRSocketInteractor socket;
    private bool isCorrectlyFilled = false;
    private bool isLocked = false;

    public bool IsCorrectlyFilled => isCorrectlyFilled;
    public string AcceptedTag => acceptedTag;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1f;
            }
        }
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnSelectEntered);
        socket.selectExited.AddListener(OnSelectExited);
        socket.hoverEntered.AddListener(OnHoverEntered);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSelectEntered);
        socket.selectExited.RemoveListener(OnSelectExited);
        socket.hoverEntered.RemoveListener(OnHoverEntered);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (isLocked) return;

        if (!args.interactableObject.transform.CompareTag(acceptedTag))
        {
            PlaySound(wrongSound);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log($"Object snapped: {args.interactableObject.transform.name}, Tag: {args.interactableObject.transform.tag}, Expected: {acceptedTag}");

        if (args.interactableObject.transform.CompareTag(acceptedTag))
        {
            isCorrectlyFilled = true;
            PlaySound(correctSound);
            if (correctParticles != null)
            {
                correctParticles.gameObject.SetActive(true); // if it's hidden
                correctParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                correctParticles.Play();
            }
            LockSocket(args.interactableObject);
            Debug.Log($"MatchingSocket correctly filled with tag: {acceptedTag}");
            puzzleManager.CheckPuzzleState();
        }
        else
        {
            // Wrong object snapped in — place back at spawn
            StartCoroutine(ReturnWrongObject(args.interactableObject));
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (isLocked) return;
        if (isCorrectlyFilled)
        {
            isCorrectlyFilled = false;
            puzzleManager.CheckPuzzleState();
        }
    }

    private System.Collections.IEnumerator ReturnWrongObject(IXRSelectInteractable interactable)
    {
        yield return null; // wait one frame

        // Force release from socket
        socket.interactionManager.SelectExit(socket, interactable);

        // Reset position
        var returnScript = interactable.transform.GetComponent<ReturnToSpawn>();
        if (returnScript != null)
        {
            returnScript.ResetToSpawn();
        }

        PlaySound(wrongSound);
    }

    private void LockSocket(IXRSelectInteractable interactable)
    {
        isLocked = true;
        socket.socketActive = false;

        var grabInteractable = interactable.transform.GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        var rb = interactable.transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
