using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainScript : MonoBehaviour
{
    public int bullets = 20;
    public GameObject myTree;
 
    public GameObject projectile;
    public float fireforce = 1000; 
 
    public ParticleSystem jetpackSmoke;

 
    [Header("Audio")]
    public AudioClip jetpackSound;
    public AudioClip gunShotSound;
    public AudioClip emptuGunShotSound;
    public AudioClip reloadSound;
 
    [Header("Player Controller")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded {get; private set;} 
 
    [Header("Movement Settings")]
    public float moveSpeed = 8;
    public float mouseSpeed = 100;
    public float gravity = 9.8f;
    public float jumpForce = 20;
    public float dashForce = 750;
    public int maxDashes = 3;
    private int currentDashes;
    private float dashRecoverTime = 1.0f;
    private float dashRecoverTimeElapsed = 0.0f;
    [SerializeField] private GameObject[] _dashIcons;
    [SerializeField] private bool noclip = false;
    private float startFOV;
    
    [Header("Camera Settings")]
    private float pitch = 0.0f;
    private float yaw = 0.0f;
    private Vector3 velocity;
    public CharacterController characterController;
 
    [Header("Gun and Enemies")]
    public Animator gunAnimator;
    public EnemySpawner enemySpawner;
    Rigidbody rb;
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //characterController = GetComponent<CharacterController>();
 
        /* Comment to unlock the mouse cursor */
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startFOV = Camera.main.fieldOfView;

        currentDashes = maxDashes;
 
    }
 
    // Update is called once per frame
    void Update()
    {
        float horizontalValue = Input.GetAxis("Horizontal");
        float verticalValue = Input.GetAxis("Vertical");
 
        float horizontalMouseValue = Input.GetAxis("Mouse X");
        float verticalMouseValue = Input.GetAxis("Mouse Y");
 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0){
            velocity.y = 0f;
        }
 
        //two ways of jumping

        if(currentDashes < maxDashes){
            dashRecoverTimeElapsed += Time.deltaTime;
            if(dashRecoverTimeElapsed >= dashRecoverTime && isGrounded){
                currentDashes++;
                _dashIcons[currentDashes - 1].SetActive(true);
                dashRecoverTimeElapsed = 0.0f;
            }
        }
 
        // Reload
        if(Input.GetKeyDown(KeyCode.R)){
            if(bullets < 30){
                AudioManager.Instance.PlaySound(reloadSound);
                bullets = 30; 
            }
        }
        
        //transform.position += transform.forward * verticalValue* moveSpeed * Time.deltaTime + transform.right * horizontalValue* moveSpeed * Time.deltaTime;
        if(!noclip){
            Vector3 moveDirection = transform.forward * verticalValue + transform.right * horizontalValue;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else{
            characterController.Move(transform.forward * dashForce * Time.deltaTime);
        }
 
        if(!isGrounded){
            velocity.y -= gravity * Time.deltaTime;
        }
        if(!noclip)
            characterController.Move(velocity * Time.deltaTime);
 
        //naive version - does not depend on time, doesnt use the correct mouse position
 
        yaw += horizontalMouseValue * mouseSpeed * Time.deltaTime;
        pitch -= verticalMouseValue * mouseSpeed * Time.deltaTime;
 
        pitch = Mathf.Clamp(pitch, -90, 90);
 
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
 
        //transform.eulerAngles += new Vector3(-verticalMouseValue * mouseSpeed * Time.deltaTime, horizontalMouseValue * mouseSpeed * Time.deltaTime, 0);
 
 
        if (Input.GetButtonUp("Fire1")){
        
            if(bullets <= 0){
                AudioManager.Instance.PlaySound(emptuGunShotSound);
            }
            else{
                gunAnimator.SetTrigger("Shoot");
                AudioManager.Instance.PlaySound(gunShotSound);
                GameObject instantiatedBullet =
                    Instantiate(projectile, transform.position + transform.forward, transform.rotation);
                instantiatedBullet.tag = "PlayerProjectile";
                instantiatedBullet.GetComponent<Rigidbody>().AddForce(transform.forward * fireforce);
                Destroy(instantiatedBullet, 5);
                bullets--;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            Debug.Log("Dash");
            // If moving dash in the direction of the movement, otherwise dash in the direction of the camera
            if(currentDashes > 0){
                currentDashes--;
                dashRecoverTimeElapsed = 0.0f;
                _dashIcons[currentDashes].SetActive(false);
                Debug.Log("Dashes: " + currentDashes);
                StartCoroutine(Dash());
            }
        }
        if(Input.GetKeyDown(KeyCode.V)){
            enemySpawner.SpawnEnemies(15);
        }
        // Press 1
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            noclip = !noclip;
        }
    }

    public void SetJetpackForce(){
        velocity.y = jumpForce;
    }
 
    IEnumerator Dash(){
        float dashDuration = 0.4f; // Duration of the dash in seconds. Adjust this value as needed.
        transform.position += transform.forward * 3.0f; // Initial teleport

        float startTime = Time.time;
        while (dashDuration > 0)
        {
            //if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
            //    characterController.Move(moveDirection * dashForce * Time.deltaTime);
            //}
            //else{
            characterController.Move(transform.forward * dashForce * Time.deltaTime);
            //}
 
            dashDuration -= Time.deltaTime; // Decrease the remaining dash duration
            yield return null; // Wait for the next frame
        }
    }
 
    IEnumerator Jump(){
        //rb.AddForce(new Vector3(0, jumpForce, 0));
        velocity.y = jumpForce;
        yield return null;
    }
 
    // void OnGUI()
    // {
    //     if (GUI.Button(new Rect(10, 10, 100, 40), "Add bullets"))
    //     {
    //         Debug.Log("Add 10 bullets");
    //         bullets += 10+1;  //plus the one that is fired
    //     }
 
    //     GUI.contentColor = Color.red;
    //     GUI.skin.label.fontSize = 50;
    //     GUI.Label(new Rect(10, 50, 400, 100), "bullets " + bullets);
    // }
}