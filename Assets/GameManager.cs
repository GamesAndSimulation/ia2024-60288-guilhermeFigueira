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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
