using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    public GameObject tempPlayer;
    public GameObject tempEnemy;
    public Light tempLight;
    public AudioClip morphSound;
    private VideoPlayer _videoPlayer;
    
    private void Start()
    {
        tempLight.DOIntensity(1, 1f);
        _videoPlayer = GetComponent<VideoPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CutsceneEvent());
        }
    }
    
    private IEnumerator PlayCreditsVideo()
    {
        _videoPlayer.Play();
        yield return new WaitForSeconds(9.2f);
        _videoPlayer.targetCameraAlpha = 1;
        yield return new WaitForSeconds(26.96f);
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator CutsceneEvent()
    {
        tempLight.DOIntensity(0, 2f);
        AudioManager.Instance.PlaySound(morphSound, false, 1f);
        yield return new WaitForSeconds(2f);
        tempPlayer.SetActive(false);
        tempEnemy.SetActive(true);
        tempLight.DOIntensity(1f, 2f);
        StartCoroutine(PlayCreditsVideo());
        
    }
}
