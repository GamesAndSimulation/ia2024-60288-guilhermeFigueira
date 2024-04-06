using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    
    [Header("Loading")]
    public GameObject fadeImage;
    public GameObject LoadingText;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager is null");
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void ReloadFromCheckpoint(float delay)
    {
        Invoke(nameof(ReloadFromCheckpoint), delay);
    }

    public void ReloadFromCheckpoint()
    {
        fadeImage.SetActive(true);
        LoadingText.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
