using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    private bool hasStartedPlaying;
    
    void Start()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        hasStartedPlaying = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
        
    void Update()
    {
        if (_videoPlayer.isPlaying)
        {
            hasStartedPlaying = true;
        }

        if (!_videoPlayer.isPlaying && hasStartedPlaying)
        {
            Application.LoadLevel(1);
        }

    }
}
