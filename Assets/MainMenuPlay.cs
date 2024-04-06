using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPlay : MonoBehaviour
{
    
    public enum DoorType
    {
        Play,
        Quit 
    }
    
    public AudioClip deepBassHit;
    public Image fadeImage;
    public GameObject LoadingText;
    public DoorType doorType;
    

    private void Start()
    {
        fadeImage.DOFade(0, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound(deepBassHit, false, 1f);
            fadeImage.DOFade(1, 3f);
            Invoke(nameof(ShowLoadingText), 3.1f);
            foreach(AudioSource audio in AudioManager.Instance.audioSources)
            {
                audio.DOFade(0, 3f);
            }
            if (doorType == DoorType.Quit)
            {
                Invoke(nameof(Application.Quit), 3.5f);
            }
            else
            {
                Invoke(nameof(LoadLevel), 3.5f);
            }
        }
    }
    
    private void ShowLoadingText()
    {
        LoadingText.SetActive(true);
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene("PlatformCourse");
    }
}
