using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSocketInteractor))]
public class MatchingSocket : MonoBehaviour
{
    [SerializeField] private MineralType acceptedType;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private MatchingPuzzleManager puzzleManager;

    private XRSocketInteractor socket;
    private bool isCorrectlyFilled = false;
    private bool isLocked = false;

    public bool IsCorrectlyFilled => isCorrectlyFilled;
    public MineralType AcceptedType => acceptedType;

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
                audioSource.spatialBlend = 1f; // 3D sound
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

        MatchingMineral mineral = args.interactableObject.transform.GetComponent<MatchingMineral>();
        if (mineral != null && mineral.Type != acceptedType)
        {
            PlaySound(wrongSound);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        MatchingMineral mineral = args.interactableObject.transform.GetComponent<MatchingMineral>();
        if (mineral != null && mineral.Type == acceptedType)
        {
            isCorrectlyFilled = true;
            PlaySound(correctSound);
            LockSocket(args.interactableObject);
            Debug.Log($"MatchingSocket correctly filled {acceptedType}");
            puzzleManager.CheckPuzzleState();
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