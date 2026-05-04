using UnityEngine;
using UnityEngine.InputSystem;

public class Quit : MonoBehaviour
{
    public InputActionReference action;

    private void OnEnable()
    {
        action.action.Enable();
        action.action.performed += OnPerformed;
    }

    private void OnDisable()
    {
        action.action.performed -= OnPerformed;
        action.action.Disable();
    }

    private void OnPerformed(InputAction.CallbackContext ctx)
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }
}
