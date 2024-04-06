using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalCutsceneTrigger : MonoBehaviour
{
    
    public Image fadeImage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CutsceneEvent());
        }
    }
    
    private IEnumerator CutsceneEvent()
    {
        fadeImage.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Finale");
    }
}
