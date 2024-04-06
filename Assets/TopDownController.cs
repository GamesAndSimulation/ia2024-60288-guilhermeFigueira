using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    public Rigidbody rb;
    public float moveSpeed = 1f;
    
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (horizontalInput != 0 || verticalInput != 0)
        {
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
            transform.forward = moveDirection;
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.VelocityChange);
        }
    }
}
