using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSocketInteractor))]
public class MatchingSocket : MonoBehaviour
{
    [SerializeField] private MineralType acceptedType;

    private XRSocketInteractor socket;
    private bool isCorrectlyFilled = false;

    public bool IsCorrectlyFilled => isCorrectlyFilled;
    public MineralType AcceptedType => acceptedType;

    private void Awake()
    {
        socket = GetComponent<XRSocketInteractor>();
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(OnSelectEntered);
        socket.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(OnSelectEntered);
        socket.selectExited.RemoveListener(OnSelectExited);
    }

    // Filter so only the correct mineral can be socketed
    public bool CanAcceptInteractable(IXRSelectInteractable interactable)
    {
        MatchingMineral mineral = interactable.transform.GetComponent<MatchingMineral>();
        return mineral != null && mineral.Type == acceptedType;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        MatchingMineral mineral = args.interactableObject.transform.GetComponent<MatchingMineral>();
        if (mineral != null && mineral.Type == acceptedType)
        {
            isCorrectlyFilled = true;
            MatchingPuzzleManager.Instance?.CheckPuzzleState();
        }
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        if (isCorrectlyFilled)
        {
            isCorrectlyFilled = false;
            MatchingPuzzleManager.Instance?.CheckPuzzleState();
        }
    }
}