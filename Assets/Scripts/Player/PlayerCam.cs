using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;
    float zRotation;

    public PlayerScript playerScript;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.5f;
    private float shakeTimer;

    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update(){
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.fixedDeltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.fixedDeltaTime;

        Debug.Log($"Mouse X: {mouseX}, Mouse Y: {mouseY}");

        yRotation += mouseX;
        zRotation = playerScript.currentRoll;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Debug.Log($"X Rotation: {xRotation}, Y Rotation: {yRotation}, Z Rotation: {zRotation}");


        transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);

        //if(Input.GetMouseButtonUp(0)){
        //    Shake();
        //}

        //if(shakeTimer > 0){
        //    Quaternion shakeOffset = Quaternion.Euler(PerlinShake() * shakeMagnitude);
        //    transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation) * shakeOffset;
        //    shakeTimer -= Time.deltaTime;
        //}
    }

    public void Shake(){
        shakeTimer = shakeDuration;
    }

   private Vector3 PerlinShake(){
    float x = (Mathf.PerlinNoise(Time.time, 0f) * 2 - 1) * shakeMagnitude;
    float y = (Mathf.PerlinNoise(0f, Time.time) * 2 - 1) * shakeMagnitude;
    return new Vector3(x, y, 0f);
} 

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

}
