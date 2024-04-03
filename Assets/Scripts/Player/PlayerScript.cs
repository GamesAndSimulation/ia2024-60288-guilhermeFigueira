using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerScript : MonoBehaviour
{
    public int bullets = 20;
    public float health = 100;
    public int enemyKillCount = 0;

    [Header("References")]
    public GameObject deathScreen;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI bulletsText;
    public PostProcessVolume _volume;
    Vignette _vignette;
    public float bloodIntensity = 0;
 
    [Header("Gun")]
    public GameObject projectile;
    public float fireforce = 1000; 
 
    [Header("Jetpack")]
    public ParticleSystem jetpackSmoke;
 
    [Header("Audio")]
    public AudioClip jetpackSound;
    public AudioClip gunShotSound;
    public AudioClip emptuGunShotSound;
    public AudioClip reloadSound;
 
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    private float initialWalkSpeed;

    public float maxYSpeed;
    
    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float groundDrag;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public Camera cam;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        dashing,
        air
    }

    public bool dashing;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded { get; private set;}
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Gun and Enemies")]
    public Animator gunAnimator;
    public EnemySpawner enemySpawner;

    [Header("Quake camera rolling")]
    public float rollSpeed;
    public float maxRoll;
    public float tiltAmount = 5.0f;
    public float currentTilt {get; private set;}
    public float currentRoll {get; private set;}
    public bool otherside = false;

 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        initialWalkSpeed = walkSpeed;
        isReloading = false;
 
        _volume.profile.TryGetSettings<Vignette>(out _vignette);

        if(!_vignette){
            Debug.LogError("No vignette found");
        }
        else{
            _vignette.enabled.Override(false);
        }
 
        /* Comment to unlock the mouse cursor */
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
 
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight / 2 + 0.1f), Color.red);

        HandleInputs();
        CameraTilting();
        StateHandler();
        SpeedControl();
        
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void CameraTilting()
{
    // Horizontal movement tilt
    float horizontalInput = Input.GetAxisRaw("Horizontal");
    int direction = otherside ? -1 : 1;
    float rollThisFrame = direction * horizontalInput * rollSpeed * Time.deltaTime;
    currentRoll = Mathf.Clamp(currentRoll + rollThisFrame, -maxRoll, maxRoll);
    
    // Returning the camera to neutral position when there's no horizontal input
    if (horizontalInput == 0 && currentRoll != 0)
    {
        // Adjusting the currentRoll towards 0 based on rollSpeed
        currentRoll = Mathf.MoveTowards(currentRoll, 0, Time.deltaTime * rollSpeed);
    }

    // Vertical jumping tilt
    if (!grounded)
    {
        float normalizedVelocityY = Mathf.Clamp(rb.velocity.y / 3.0f, -1f, 1f);
        currentTilt = Mathf.Lerp(currentTilt, tiltAmount * normalizedVelocityY, Time.deltaTime * rollSpeed);
    }
    else
    {
        // Gradually return to neutral tilt when grounded
        currentTilt = Mathf.Lerp(currentTilt, 0, Time.deltaTime * rollSpeed * 5);
    }

    // Applying the calculated tilt to the camera's X-axis
    // Ensuring the tilt effect is applied correctly in addition to any existing roll
    Camera.main.transform.localEulerAngles = new Vector3(currentTilt, Camera.main.transform.localEulerAngles.y, Camera.main.transform.localEulerAngles.z);
}

    private void FixedUpdate()
    {
        MovePlayer();
        if (Input.GetKey(KeyCode.Space))
        {
            walkSpeed = Mathf.Clamp(walkSpeed + 0.1f, initialWalkSpeed, sprintSpeed);
            if(readyToJump && grounded){

                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
        if (!grounded && !jetpackManager.usingJetpack)
        {
            ApplyAdditionalGravityForce();
        }
    }

    void ApplyAdditionalGravityForce(){
        float additionalGravityForce = 9.81f;
        float additionalGravityFactor = 2f;
        rb.AddForce(Vector3.down * additionalGravityForce * additionalGravityFactor, ForceMode.Acceleration);
    }

    void HandleInputs(){

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //if(horizontalInput != 0 || verticalInput != 0){
        //   gunAnimator.SetBool("isWalking", true);
        //}
        //else{
        //   gunAnimator.SetBool("isWalking", false);
        //}

        if(Input.GetKeyUp(KeyCode.Space)){
            walkSpeed = initialWalkSpeed;
        }
        if (Input.GetButtonUp("Fire1")){

                    if(bullets <= 0){
                        AudioManager.Instance.PlaySound(emptuGunShotSound);
                    }
                    else{
                        gunAnimator.SetTrigger("Shoot");
                        AudioManager.Instance.PlaySound(gunShotSound, false, 1f);
                        GameObject instantiatedBullet =
                            Instantiate(projectile, Camera.main.transform.position + Camera.main.transform.forward, Camera.main.transform.rotation);
                        instantiatedBullet.tag = "PlayerProjectile";
                        instantiatedBullet.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * fireforce, ForceMode.Impulse);
                        Destroy(instantiatedBullet, 5);
                        bullets--;
                        bulletsText.text = String.Format("{0} / 14", bullets);
                    }
                }


        // Reload
        if(Input.GetKeyDown(KeyCode.R)){
            if(bullets < 14)
            {
                StartCoroutine(Reload());
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            Debug.Log("Dash");
        }
        if(Input.GetKeyDown(KeyCode.V)){
            enemySpawner.SpawnEnemies(15);
        }
        // Press 1
        if(Input.GetKeyDown(KeyCode.Alpha1)){
        }
    }

    private bool isReloading;
    
    private IEnumerator Reload()
    {
        isReloading = true;
        gunAnimator.SetTrigger("Reload");
        AnimationClip reloadAnimation = gunAnimator.runtimeAnimatorController.animationClips[2]; // 1 is the index of the reload animation
        AudioManager.Instance.PlaySound(reloadSound);
        yield return new WaitForSeconds(reloadAnimation.length);
        bullets = 14; 
        bulletsText.text = String.Format("{0} / 14", bullets);
        isReloading = false;
    }


    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;
    private void StateHandler()
    {
        // Mode - Dashing
        if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(KeyCode.LeftShift))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;

            if (desiredMoveSpeed < sprintSpeed)
                desiredMoveSpeed = walkSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutinesExcept();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutinesExcept();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;

    }

    private void StopAllCoroutinesExcept()
    {
        StopAllCoroutines();
        if(!isReloading)
            StartCoroutine(Reload());
    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            time += Time.deltaTime * boostFactor;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    public JetpackManager jetpackManager;

    private void MovePlayer()
    {
        if (state == MovementState.dashing) return;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded){
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
            //if(!jetpackManager.usingJetpack){
            //    rb.AddForce(Vector3.down * 10f, ForceMode.Force);
            //}
        }
    }

    private void SpeedControl()
    {
        // limiting speed on ground or in air
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

        // limit y vel
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        UpwardsForce(jumpForce);
    }

    public void UpwardsForce(float force){

        rb.AddForce(transform.up * force , ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool isDamageEffectRunning = false;

    public void ChangeHealth(int healthIncrement)
    {
        health -= healthIncrement;
        health = Mathf.Clamp(health, 0, 100);
        healthText.text = health.ToString();
        if (health <= 0)
        {
            Die();
        }
    }
    


    private void Die()
    {
        deathScreen.SetActive(true);
        GameManager.Instance.ReloadFromCheckpoint(3);
        gameObject.SetActive(false);
    }   

}