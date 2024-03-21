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

    [Header("Dashing")]
    public float dashSpeed;
    public float dashUpwardForce;
    public float dashDuration;

    [Header("CameraEffects")]
    public PlayerCam cam;
    public float dashFov;
    private float defaultFov;

    [Header("Cooldown")]
    public float dashCooldown;
    private float dashCooldownTimer;

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
    }

    private void Dash()
    {
        if(dashCooldownTimer > 0)
            return;
        else
            dashCooldownTimer = dashCooldown;

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
