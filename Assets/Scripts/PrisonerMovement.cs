using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    private CharacterController _characterController;
    private Animator _animator;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(moveDirection.magnitude) * speed;

        _characterController.SimpleMove(moveDirection * magnitude);
        
        if (moveDirection != Vector3.zero)
        {
            _animator.SetBool("Moving", true);
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }

    }
}
