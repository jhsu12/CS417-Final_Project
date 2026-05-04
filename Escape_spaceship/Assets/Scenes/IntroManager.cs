using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer videoPlayer;

    [Header("VR Screen")]
    public Transform screenQuad;       // drag your Quad here
    public Transform playerHead;       // drag XR Origin Camera here
    public float screenDistance = 4f;  // how far in front of player
    public float screenWidth = 8f;     // quad scale X
    public float screenHeight = 4.5f;  // quad scale Y (16:9 ratio)

    [Header("Scene")]
    public string gameSceneName = "TestingXR";

    private bool videoFinished = false;

    void Start()
    {
        PositionScreen();
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void PositionScreen()
    {
        if (screenQuad == null || playerHead == null) return;

        Vector3 forward = playerHead.forward;
        forward.y = 0;
        forward.Normalize();

        screenQuad.position = playerHead.position + (forward * screenDistance);
        screenQuad.rotation = Quaternion.LookRotation(forward);
        screenQuad.localScale = new Vector3(screenWidth, screenHeight, 1f);
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void Update()
    {
        // Skip intro with new Input System
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            videoPlayer.Stop();
            SceneManager.LoadScene(gameSceneName);
        }
    }
}