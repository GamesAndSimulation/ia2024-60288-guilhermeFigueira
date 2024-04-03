using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Dashing : MonoBehaviour
{

    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerScript playerScript;
    public GameObject[] dashIcons;

    [Header("Dashing")]
    public float dashSpeed;
    public float dashUpwardForce;
    public float dashDuration;
    public int dashesCount;

    [Header("CameraEffects")]
    public PlayerCam cam;
    public float dashFov;
    private float defaultFov;

    [Header("Cooldown")]
    public float dashCooldown;
    private float dashCooldownTimer;

    [Header("Regen")]
    public float dashRegen;
    private float dashRegenTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.LeftShift;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = GetComponent<PlayerScript>();
        defaultFov = Camera.main.fieldOfView;
    }

    private void Update()
    {
        if (Input.GetKeyDown(dashKey)) 
        {
            Dash();
        }
        if(dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;
        if(dashRegenTimer > 0){
            dashRegenTimer -= Time.deltaTime;
        }
        else{
            if(dashesCount < 2){
                dashRegenTimer = dashRegen;
                dashesCount++;
                dashIcons[dashesCount-1].SetActive(true);
            }
            else if(dashesCount == 2){
                dashesCount++;
                dashIcons[dashesCount-1].SetActive(true);
            }
        }

    }

    private void Dash()
    {
        if(dashesCount <= 0)
            return;

        if(dashCooldownTimer > 0)
            return;
        else
            dashCooldownTimer = dashCooldown;

        dashesCount--;
        dashIcons[dashesCount].SetActive(false);
        dashRegenTimer = dashRegen;
        playerScript.dashing = true;
        cam.DoFov(dashFov);

        Vector3 forceToApply = orientation.forward * dashSpeed + orientation.up * dashUpwardForce;
        delayedForceToApply = forceToApply;

        Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDash()
    {
        playerScript.dashing = false;
        cam.DoFov(defaultFov);
    }


}
