using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;

    [SerializeField] float movementSpeed = 1;
    [SerializeField] float gravity = 1f;
    public Camera cam;

    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        CharacterMovement();
        ProcessJumping();
    }

    void CharacterMovement()
    {
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed;
        float vertical = Input.GetAxis("Vertical") * movementSpeed;
        characterController.Move((cam.transform.right * horizontal + cam.transform.forward * vertical) * Time.deltaTime);
    }

    void ProcessJumping()
    {
        if (characterController.isGrounded)
        {
            moveDirection.y = 0;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}
