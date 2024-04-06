using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameStartEvent : MonoBehaviour
{
    public Image fadeImage;
    public AudioClip robotVoice;
    public GameObject gate;
    public Light blinkningLight;
    private float lightTimer;
    public float lightBlinkTime;
    
    void Start()
    {
        lightTimer = lightBlinkTime;
        fadeImage.DOFade(0, 5f);
        AudioManager.Instance.PlaySound(robotVoice, false, 3f);
        Invoke(nameof(OpenGate), robotVoice.length + 1f);
    }

    private void Update()
    {
        lightTimer -= Time.deltaTime;
        if (lightTimer <= 0)
        {
            blinkningLight.enabled = !blinkningLight.enabled;
            lightTimer = lightBlinkTime;
        }
    }

    void OpenGate()
    {
        gate.transform.DOMoveY(-9.27f, 3f).SetEase(Ease.InOutQuad);
    }

}
